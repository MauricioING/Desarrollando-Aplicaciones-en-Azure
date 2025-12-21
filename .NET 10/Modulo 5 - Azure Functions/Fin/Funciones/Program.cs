using Funciones.Datos;
using Funciones.Servicios;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opciones => 
        opciones.UseSqlServer("name=DefaultConnection"));

builder.Services.AddTransient<IServicioCorreos, ServicioCorreos>();

builder.Services.AddAzureClients(builderInterno =>
{
    builderInterno.AddBlobServiceClient(builder.Configuration["ConnectionStrings:AzureStorage"]);
});

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
