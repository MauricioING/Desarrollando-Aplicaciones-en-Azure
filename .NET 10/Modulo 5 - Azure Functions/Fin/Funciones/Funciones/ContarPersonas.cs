using Funciones.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Funciones.Funciones;

public class ContarPersonas
{
    private readonly ILogger<ContarPersonas> _logger;

    public ContarPersonas(ILogger<ContarPersonas> logger)
    {
        _logger = logger;
    }

    [Function("ContarPersonas")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var body = await new StreamReader(req.Body).ReadToEndAsync();
        var personas = JsonSerializer.Deserialize<List<Persona>>(body,
                 new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        return new OkObjectResult($"La cantidad de personas enviadas fue: {personas.Count}");
    }
}