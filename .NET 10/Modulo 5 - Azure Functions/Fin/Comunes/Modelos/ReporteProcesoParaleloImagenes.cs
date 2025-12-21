using System;
using System.Collections.Generic;
using System.Text;

namespace Comunes.Modelos
{
    public class ReporteProcesoParaleloImagenes
    {
        public Guid Id { get; set; }
        public required string CorreoUsuario { get; set; }
        public List<ImagenProcesoParalelo> Imagenes { get; set; } = [];
    }

    public enum EstadoImagenProcesoParalelo
    {
        SinOptimizar = 1,
        Optimizada = 2
    }

    public class ImagenProcesoParalelo
    {
        public Guid Id { get; set; }
        public Guid ReporteProcesoParaleloImagenesId { get; set; }
        public ReporteProcesoParaleloImagenes ReporteProcesoParaleloImagenes { get; set; } = null!;
        public EstadoImagenProcesoParalelo EstadoImagenProcesoParalelo { get; set; } = EstadoImagenProcesoParalelo.SinOptimizar;
        public required string Url { get; set; }
    }

}
