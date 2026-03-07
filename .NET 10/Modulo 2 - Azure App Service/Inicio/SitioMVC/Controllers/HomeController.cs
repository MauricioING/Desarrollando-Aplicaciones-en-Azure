using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SitioMVC.Models;

namespace SitioMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;

        public HomeController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public IActionResult Index()
        {
            var cantidadPersonas = configuration.GetValue<int>("cantidad-personas");
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
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
