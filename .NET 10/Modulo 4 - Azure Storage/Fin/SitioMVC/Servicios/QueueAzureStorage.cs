using Azure.Storage.Queues;

namespace SitioMVC.Servicios
{
    public class QueueAzureStorage : IQueueServicio
    {
        private readonly string connectionString;

        public QueueAzureStorage(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorageConnection")!;
        }

        public async Task EscribirMensaje(string queue, string mensaje)
        {
            QueueClient queueClient = new QueueClient(connectionString, queue);
            await queueClient.SendMessageAsync(mensaje);
        }
    }
}
