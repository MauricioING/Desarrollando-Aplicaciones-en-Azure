using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Funciones.Funciones;

public class FuncionDePrueba
{
    private readonly ILogger<FuncionDePrueba> _logger;

    public FuncionDePrueba(ILogger<FuncionDePrueba> logger)
    {
        _logger = logger;
    }

    [Function("FuncionDePrueba")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Te doy la bienvenida a las funciones de Azure");
    }
}