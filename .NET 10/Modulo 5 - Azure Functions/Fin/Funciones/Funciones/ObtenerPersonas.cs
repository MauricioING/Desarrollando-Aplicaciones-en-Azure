using Funciones.Datos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Funciones.Funciones;

public class ObtenerPersonas
{
    private readonly ILogger<ObtenerPersonas> _logger;
    private readonly ApplicationDbContext context;

    public ObtenerPersonas(ILogger<ObtenerPersonas> logger, ApplicationDbContext context)
    {
        _logger = logger;
        this.context = context;
    }

    [Function("ObtenerPersonas")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", 
        Route = "ObtenerPersonas")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var personas = await context.Personas.ToListAsync();
        return new OkObjectResult(personas);
    }
}