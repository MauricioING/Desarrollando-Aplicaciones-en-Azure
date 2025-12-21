using Funciones.Datos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Funciones.Funciones.OptimizadorGrupoDeImagenes
{
    internal class ObtenerURLs(ApplicationDbContext context)
    {
        [Function(nameof(ObtenerURLs))]
        public async Task<string[]> Run([ActivityTrigger] Guid jobId, FunctionContext executionContext)
        {
            var reporte = await context.ReportesProcesoParaleloImagenes
                .Include(x => x.Imagenes.Where(img => 
                img.EstadoImagenProcesoParalelo == Comunes.Modelos.EstadoImagenProcesoParalelo.SinOptimizar))
                .FirstOrDefaultAsync(x => x.Id == jobId);

            if (reporte is null)
            {
                return [];
            }

            return reporte.Imagenes.Select(x => x.Url).ToArray();
        }
    }

}
