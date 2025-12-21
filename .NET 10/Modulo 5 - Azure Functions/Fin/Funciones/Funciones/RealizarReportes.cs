using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Queues.Models;
using Comunes.Modelos;
using Funciones.Datos;
using Funciones.Modelos;
using Funciones.Servicios;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Text.Json;

namespace Funciones.Funciones;

public class RealizarReportes
{
    private readonly ILogger<RealizarReportes> _logger;
    private readonly ApplicationDbContext context;
    private readonly IServicioCorreos servicioCorreos;
    private readonly BlobServiceClient blobServiceClient;

    public RealizarReportes(ILogger<RealizarReportes> logger,
        ApplicationDbContext context, IServicioCorreos servicioCorreos, BlobServiceClient blobServiceClient)
    {
        _logger = logger;
        this.context = context;
        this.servicioCorreos = servicioCorreos;
        this.blobServiceClient = blobServiceClient;
    }

    [Function(nameof(RealizarReportes))]
    public async Task Run([QueueTrigger("reportes-personas", Connection = "AzureStorage")] QueueMessage message)
    {
        _logger.LogInformation("C# Queue trigger function processed: {messageText}", message.MessageText);
        var reporte = JsonSerializer.Deserialize<RealizarReporteMensajeDTO>(message.MessageText)!;
        var reporteDB = await context.Reportes.FirstOrDefaultAsync(x => x.Id == reporte.ReporteId);

        if (reporteDB is null)
        {
            _logger.LogError($"No existe el reporte con Id: {reporte.ReporteId}");
            return;
        }

        var personas = await context.Personas.ToListAsync();
        var csv = GenerarCsv(personas);
        var nombreArchivo = $"personas-reporte-{reporte.ReporteId}.csv";
        string url = await SubirCsvABlob(csv, nombreArchivo);

        reporteDB.ReporteURL = url;
        reporteDB.ReporteEstatus = ReporteStatus.Completado;
        await context.SaveChangesAsync();

        await servicioCorreos.EnviarReportePersonas(url, reporte.CorreoUsuario);

    }

    private async Task<string> SubirCsvABlob(string csv, string nombreArchivo)
    {
        var cliente = blobServiceClient.GetBlobContainerClient("reportes-personas");
        await cliente.CreateIfNotExistsAsync();
        cliente.SetAccessPolicy(PublicAccessType.Blob);

        var blobClient = cliente.GetBlobClient(nombreArchivo);
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));
        await blobClient.UploadAsync(stream, overwrite: true);
        return blobClient.Uri.ToString();
    }


    private string GenerarCsv(List<Persona> personas)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Id,Nombre,FotoURL,Correo,FechaNacimiento");
        foreach (var persona in personas)
        {
            sb.AppendLine($"{persona.Id},{persona.Nombre},{persona.FotoURL},{persona.Correo},{persona.FechaNacimiento}");
        }
        return sb.ToString();
    }

}