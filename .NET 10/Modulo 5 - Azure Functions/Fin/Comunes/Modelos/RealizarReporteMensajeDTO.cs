using System;
using System.Collections.Generic;
using System.Text;

namespace Comunes.Modelos
{
    public class RealizarReporteMensajeDTO
    {
        public Guid ReporteId { get; set; }
        public required string CorreoUsuario { get; set; }
    }
}
