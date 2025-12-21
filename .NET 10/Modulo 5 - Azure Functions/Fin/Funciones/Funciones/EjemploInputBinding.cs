using Funciones.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Extensions.Logging;

namespace Funciones.Funciones;

public class EjemploInputBinding
{
    private readonly ILogger<EjemploInputBinding> _logger;

    public EjemploInputBinding(ILogger<EjemploInputBinding> logger)
    {
        _logger = logger;
    }

    [Function("EjemploInputBinding")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,

        [SqlInput(commandText: "select * from Personas",
        commandType: System.Data.CommandType.Text,
        parameters: "",
        connectionStringSetting: "DefaultConnection")] IEnumerable<Persona> personas
        )
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult(personas);
    }
}