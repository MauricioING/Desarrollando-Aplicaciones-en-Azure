using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Queues.Models;
using Comunes.Modelos;
using Funciones.Datos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using System;
using System.Text.Json;

namespace Funciones.Funciones;

public class OptimizarImagenesPersonas
{
    private readonly ILogger<OptimizarImagenesPersonas> _logger;
    private readonly BlobServiceClient blobServiceClient;
    private readonly ApplicationDbContext context;

    public OptimizarImagenesPersonas(ILogger<OptimizarImagenesPersonas> logger, BlobServiceClient blobServiceClient,
        ApplicationDbContext context)
    {
        _logger = logger;
        this.blobServiceClient = blobServiceClient;
        this.context = context;
    }

    [Function(nameof(OptimizarImagenesPersonas))]
    [QueueOutput("optimizar-imagenes-personas-poison", Connection = "AzureStorage")]
    public async Task<string[]> Run([QueueTrigger("optimizar-imagenes-personas", Connection = "AzureStorage")] QueueMessage message)
    {
        _logger.LogInformation("C# Queue trigger function processed: {messageText}", message.MessageText);

        var payload = JsonSerializer.Deserialize<MensajeOptimizarImagenPersona>(message.MessageText)!;
        var persona = await context.Personas.FirstOrDefaultAsync(x => x.Id == payload.PersonaId);

        if (persona is null)
        {
            _logger.LogError($"Persona con id {payload.PersonaId} no encontrada");
            return [message.MessageText];
        }

        Stream stream = await ObtenerStream(payload.ImagenURL);
        var nombreArchivo = Path.GetFileNameWithoutExtension(payload.ImagenURL);
        var url = await OptimizarYSubirImagenAlBlob(stream, nombreArchivo);
        persona.FotoURL = url;
        await context.SaveChangesAsync();
        return [];
    }

    private async Task<Stream> ObtenerStream(string imagenURL)
    {
        var cliente = blobServiceClient.GetBlobContainerClient("personas");
        await cliente.CreateIfNotExistsAsync();
        cliente.SetAccessPolicy(PublicAccessType.Blob);
        var nombreArchivo = Path.GetFileName(imagenURL);
        var blobCliente = cliente.GetBlobClient(nombreArchivo);
        var stream = new MemoryStream();
        await blobCliente.DownloadToAsync(stream);
        return stream;
    }

    private async Task<string> OptimizarYSubirImagenAlBlob(Stream streamImagenOriginal, string nombreArchivoOriginal)
    {
        streamImagenOriginal.Position = 0;
        using var image = await Image.LoadAsync(streamImagenOriginal);

        var encoder = new WebpEncoder
        {
            Quality = 75
        };

        var cliente = blobServiceClient.GetBlobContainerClient("personas-optimizado");
        await cliente.CreateIfNotExistsAsync();
        cliente.SetAccessPolicy(PublicAccessType.Blob);

        var nombreArchivo = $"{Path.GetFileNameWithoutExtension(nombreArchivoOriginal)}.webp";

        var blobClient = cliente.GetBlobClient(nombreArchivo);

        using var stream = new MemoryStream();
        await image.SaveAsWebpAsync(stream);
        stream.Position = 0;
        var blobHttpHeaders = new BlobHttpHeaders();
        blobHttpHeaders.ContentType = "image/webp";
        await blobClient.UploadAsync(stream, blobHttpHeaders);
        return blobClient.Uri.ToString();
    }


}