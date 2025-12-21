using Azure;
using Azure.Data.Tables;
using SitioMVC.Models;

namespace SitioMVC.Servicios
{
    public class RepositorioPerfiles : IRepositorioPerfiles
    {
        private readonly TableClient _tableClient;

        public RepositorioPerfiles(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AzureStorageConnection")!;
            var nombreTabla = "perfiles";
            _tableClient = new TableClient(connectionString, nombreTabla);
        }

        public async Task Crear(PerfilPersona perfil)
        {
            await _tableClient.AddEntityAsync(perfil);
        }

        public async Task<PerfilPersona?> Obtener(string partitionKey, string rowKey)
        {
            try
            {
                var response = await _tableClient.GetEntityAsync<PerfilPersona>(partitionKey, rowKey);
                return response.Value;
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return null;
            }
        }

        public async Task<List<PerfilPersona>> ObtenerPerfiles(string partitionKey)
        {
            var perfiles = await _tableClient.QueryAsync<PerfilPersona>(
                e => e.PartitionKey == partitionKey
            ).ToListAsync();

            return perfiles;
        }

        public async Task Actualizar(PerfilPersona perfil)
        {
            await _tableClient.UpdateEntityAsync(perfil, perfil.ETag, TableUpdateMode.Replace);
        }

        public async Task Borrar(string partitionKey, string rowKey)
        {
            await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }
    }
}
