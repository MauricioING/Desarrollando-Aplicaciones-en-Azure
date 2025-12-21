
using Funciones.Funciones.OptimizadorGrupoDeImagenes;

namespace Funciones.Servicios
{
    public interface IServicioCorreos
    {
        Task EnviarMensajeDeCumple(string nombre, string correo);
        Task EnviarReporteImagenesOptimizadas(ResultadoOptimizarGrupoDeImagenes modelo, string correo);
        Task EnviarReportePersonas(string url, string correo);
    }
}