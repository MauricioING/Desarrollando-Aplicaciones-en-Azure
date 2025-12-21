using SitioMVC.Models;

namespace SitioMVC.Servicios
{
    public interface IRepositorioPerfiles
    {
        Task Actualizar(PerfilPersona perfil);
        Task Borrar(string partitionKey, string rowKey);
        Task Crear(PerfilPersona perfil);
        Task<PerfilPersona?> Obtener(string partitionKey, string rowKey);
        Task<List<PerfilPersona>> ObtenerPerfiles(string partitionKey);
    }
}