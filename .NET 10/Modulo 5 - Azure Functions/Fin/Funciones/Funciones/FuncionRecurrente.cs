using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Funciones.Funciones;

public class FuncionRecurrente
{
    private readonly ILogger _logger;

    public FuncionRecurrente(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<FuncionRecurrente>();
    }

    [Function("FuncionRecurrente")]
    public void Run([TimerTrigger("*/5 * * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation("C# Timer trigger function executed at: {executionTime}", DateTime.Now);
        
        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation("Next timer schedule at: {nextSchedule}", myTimer.ScheduleStatus.Next);
        }
    }
}