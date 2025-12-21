using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Funciones.Funciones.OptimizadorGrupoDeImagenes
{
    internal class OptimizadorImagen
    {
        private readonly ILogger<OptimizadorImagen> _logger;
        private readonly BlobServiceClient blobServiceClient;

        public OptimizadorImagen(ILogger<OptimizadorImagen> logger, BlobServiceClient blobServiceClient)
        {
            _logger = logger;
            this.blobServiceClient = blobServiceClient;
        }

        [Function(nameof(OptimizadorImagen))]
        public async Task<string> Run([ActivityTrigger] string imagenUrl, FunctionContext executionContext)
        {
            _logger.LogInformation("C# Queue trigger function processed: {imagenUrl}", imagenUrl);

            Stream stream = await ObtenerStream(imagenUrl);
            var nombreArchivo = Path.GetFileNameWithoutExtension(imagenUrl);
            var url = await OptimizarYSubirImagenAlBlob(stream, nombreArchivo);
            return url;
        }

        private async Task<Stream> ObtenerStream(string imagenURL)
        {
            var cliente = blobServiceClient.GetBlobContainerClient("paralelo");
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

            var cliente = blobServiceClient.GetBlobContainerClient("paralelo-optimizado");
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

}
