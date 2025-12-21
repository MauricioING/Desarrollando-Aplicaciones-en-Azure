using Funciones.Datos;
using Funciones.Servicios;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Funciones.Funciones;

public class FelizCumple
{
    private readonly ILogger _logger;
    private readonly ApplicationDbContext context;
    private readonly IServicioCorreos servicioCorreos;

    public FelizCumple(ILoggerFactory loggerFactory, ApplicationDbContext context,
        IServicioCorreos servicioCorreos)
    {
        _logger = loggerFactory.CreateLogger<FelizCumple>();
        this.context = context;
        this.servicioCorreos = servicioCorreos;
    }

    [Function("FelizCumple")]
    public async Task Run([TimerTrigger("0 0 8 * * *"
#if DEBUG
        , RunOnStartup = true
#endif
        )] TimerInfo myTimer)
    {
        _logger.LogInformation("C# Timer trigger function executed at: {executionTime}", DateTime.Now);

        var hoy = DateTime.Today;
        var personasQueCumplen = await context.Personas.Where(x => x.FechaNacimiento.Day == hoy.Day && 
        x.FechaNacimiento.Month == hoy.Month)
            .ToListAsync();

        foreach (var persona in personasQueCumplen)
        {
            await servicioCorreos.EnviarMensajeDeCumple(persona.Nombre, persona.Correo!);
        }

    }
}