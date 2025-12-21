using System;
using System.Collections.Generic;
using System.Text;

namespace Comunes.Modelos
{
    public class Reporte
    {
        public Guid Id { get; set; }
        public ReporteStatus ReporteEstatus { get; set; } = ReporteStatus.Pendiente;
        public required string CorreoUsuario { get; set; }
        public string? ReporteURL { get; set; }
    }

    public enum ReporteStatus
    {
        Pendiente = 1,
        Completado = 2
    }

}
