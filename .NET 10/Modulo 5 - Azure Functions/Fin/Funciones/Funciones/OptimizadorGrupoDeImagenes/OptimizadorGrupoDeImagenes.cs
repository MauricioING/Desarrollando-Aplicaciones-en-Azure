using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace Funciones.Funciones.OptimizadorGrupoDeImagenes;

public static class OptimizadorGrupoDeImagenes
{
    [Function(nameof(OptimizadorGrupoDeImagenes))]
    public static async Task<ResultadoOptimizarGrupoDeImagenes> RunOrchestrator(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        ILogger logger = context.CreateReplaySafeLogger(nameof(OptimizadorGrupoDeImagenes));
        logger.LogInformation("Ejecutando OptimizadorGrupoDeImagenes.");
        Guid jobId = context.GetInput<Guid>();

        var urls = await context.CallActivityAsync<string[]>(nameof(ObtenerURLs), jobId);

        if (!urls.Any())
        {
            return ResultadoOptimizarGrupoDeImagenes.Error("Job no encontrado");
        }

        var tareas = new List<Task<string>>();

        for (int i = 0; i < urls.Length; i++)
        {
            Task<string> tarea = context.CallActivityAsync<string>(nameof(OptimizadorImagen), urls[i]);
            tareas.Add(tarea);
        }

        var urlsOptimizadas = await Task.WhenAll(tareas);

        var resultadoOptimizarGrupoDeImagenes = new ResultadoOptimizarGrupoDeImagenes
        {
            JobId = jobId,
            Urls = urlsOptimizadas
        };


        await context.CallActivityAsync(nameof(EnviadorCorreoImagenesOptimizadas), resultadoOptimizarGrupoDeImagenes);

        return resultadoOptimizarGrupoDeImagenes;

    }


    [Function("OptimizadorGrupoDeImagenes_HttpStart")]
    public static async Task<HttpResponseData> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
        [DurableClient] DurableTaskClient client,
        FunctionContext executionContext)
    {
        ILogger logger = executionContext.GetLogger("OptimizadorGrupoDeImagenes_HttpStart");

        var body = await new StreamReader(req.Body).ReadToEndAsync();
        body = body.Replace("\"", "");
        var jobId = Guid.Parse(body);


        // Function input comes from the request content.
        string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
            nameof(OptimizadorGrupoDeImagenes), jobId);

        logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

        // Returns an HTTP 202 response with an instance management payload.
        // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
        return await client.CreateCheckStatusResponseAsync(req, instanceId);
    }
}