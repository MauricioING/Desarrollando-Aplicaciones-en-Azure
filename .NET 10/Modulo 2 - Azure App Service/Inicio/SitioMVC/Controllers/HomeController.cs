using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SitioMVC.Datos;
using SitioMVC.Models;

namespace SitioMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;

        public HomeController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {
            var personas = await context.Personas.ToListAsync();
            return View(personas);
        }

        public IActionResult Crear()
        {            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(PersonaCrearDTO personaCrearDTO)
        {
            var persona = new Persona { Nombre = personaCrearDTO.Nombre };
            context.Add(persona);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            //throw new Exception("Error en la acción Privacy");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
