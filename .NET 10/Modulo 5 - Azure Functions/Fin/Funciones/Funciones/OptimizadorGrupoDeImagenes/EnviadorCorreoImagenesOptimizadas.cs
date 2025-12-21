using Funciones.Datos;
using Funciones.Servicios;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Funciones.Funciones.OptimizadorGrupoDeImagenes
{
    internal class EnviadorCorreoImagenesOptimizadas(IServicioCorreos servicioCorreos, ILogger<OptimizadorImagen> _logger,
     ApplicationDbContext context)
    {
        [Function(nameof(EnviadorCorreoImagenesOptimizadas))]
        public async Task Run([ActivityTrigger] ResultadoOptimizarGrupoDeImagenes modelo, FunctionContext executionContext)
        {
            _logger.LogInformation("C# Queue trigger function processed: {JobId}", modelo.JobId);
            var reporte = await context.ReportesProcesoParaleloImagenes.FirstAsync(x => x.Id == modelo.JobId);
            var correo = reporte.CorreoUsuario;
            await servicioCorreos.EnviarReporteImagenesOptimizadas(modelo, correo);
        }
    }

}
