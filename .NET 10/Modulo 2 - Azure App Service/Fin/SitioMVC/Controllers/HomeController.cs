using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SitioMVC.Models;

namespace SitioMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            this.configuration = configuration;
            this._logger = logger;
        }


        public IActionResult Index()
        {
            var cantidadPersonas = configuration.GetValue<int>("cantidad-personas");

            _logger.LogInformation($"Entrando al método Index en fecha {DateTime.UtcNow}");
            _logger.LogWarning($"Advertencia: se detectó un parámetro sospechoso: {nameof(cantidadPersonas)} : {cantidadPersonas}");
            _logger.LogError("Aquí logueamos un error");

            _logger.LogError($"Procesadores disponibles: {Environment.ProcessorCount}");


            var personas = ObtenerPersonas(cantidadPersonas);
            return View(personas);
        }

        private List<Persona> ObtenerPersonas(int cantidadPersonas)
        {
            var respuesta = new List<Persona>();

            for (int i = 0; i < cantidadPersonas; i++)
            {
                respuesta.Add(new Persona
                {
                    Id = Guid.NewGuid(),
                    Nombre = $"Persona {i + 1}"
                });
            }

            return respuesta;
        }

        public IActionResult Privacy()
        {
            //throw new Exception("Esta es una prueba");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
