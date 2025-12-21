using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.DurableTask.Http;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Funciones.Funciones;

public class ObtenerYContarPersonas(IConfiguration configuration)
{
    [Function(nameof(ObtenerYContarPersonas))]
    public async Task<string> RunOrchestrator(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        ILogger logger = context.CreateReplaySafeLogger(nameof(ObtenerYContarPersonas));
        logger.LogInformation("Iniciando ObtenerYContarPersonas.");

        var url = context.GetInput<string>();

        var reintentos = new HttpRetryOptions()
        {
            MaxNumberOfAttempts = 3,
            FirstRetryInterval = TimeSpan.FromSeconds(10)
        };

        var llaveObtenerPersonas = configuration["LLAVE_OBTENER_PERSONAS"];

        var peticionObtenerPersonas = new DurableHttpRequest(HttpMethod.Get, 
                    new Uri($"{url}/api/obtenerpersonas?code={llaveObtenerPersonas}"))
        {
            HttpRetryOptions = reintentos
        };

        var respuestaObtenerPersonas = await context.CallHttpAsync(peticionObtenerPersonas);

        if (respuestaObtenerPersonas.StatusCode != System.Net.HttpStatusCode.OK)
        {
            return "Ha ocurrido un error obteniendo las personas";
        }

        var llaveContarPersonas = configuration["LLAVE_CONTAR_PERSONAS"];
        var peticionContarPersonas = new DurableHttpRequest(HttpMethod.Post, 
                new Uri($"{url}/api/ContarPersonas?code={llaveContarPersonas}"))
        {
            HttpRetryOptions = reintentos,
            Content = respuestaObtenerPersonas.Content
        };

        var respuestaContarPersonas = await context.CallHttpAsync(peticionContarPersonas);

        if (respuestaContarPersonas.StatusCode != System.Net.HttpStatusCode.OK)
        {
            return "Ha ocurrido un error contando las personas";
        }

        return respuestaContarPersonas.Content!;
    }

    [Function("ObtenerYContarPersonas_HttpStart")]
    public static async Task<HttpResponseData> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
        [DurableClient] DurableTaskClient client,
        FunctionContext executionContext)
    {
        ILogger logger = executionContext.GetLogger("ObtenerYContarPersonas_HttpStart");

        var url = req.Url.GetLeftPart(UriPartial.Authority);

        // Function input comes from the request content.
        string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
            nameof(ObtenerYContarPersonas), url);

        logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

        // Returns an HTTP 202 response with an instance management payload.
        // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
        return await client.CreateCheckStatusResponseAsync(req, instanceId);
    }
}