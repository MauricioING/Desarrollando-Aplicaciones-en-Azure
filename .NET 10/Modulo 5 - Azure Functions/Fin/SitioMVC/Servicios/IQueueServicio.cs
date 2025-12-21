
namespace SitioMVC.Servicios
{
    public interface IQueueServicio
    {
        Task EscribirMensaje(string queue, string mensaje);
    }
}