using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SitioMVC.Datos;
using SitioMVC.Models;
using SitioMVC.Servicios;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SitioMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly IRepositorioPerfiles repositorioPerfiles;
        private readonly string contenedor = "personas";

        public HomeController(ApplicationDbContext context, IAlmacenadorArchivos almacenadorArchivos,
            IRepositorioPerfiles repositorioPerfiles)
        {
            this.context = context;
            this.almacenadorArchivos = almacenadorArchivos;
            this.repositorioPerfiles = repositorioPerfiles;
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

        public async Task<IActionResult> VerDetalle(Guid id)
        {
            var persona = await context.Personas.FirstOrDefaultAsync(x => x.Id == id);

            if (persona is null)
            {
                return RedirectToAction("Index");
            }

            var perfiles = await repositorioPerfiles.ObtenerPerfiles(persona.Id.ToString());

            persona.Perfiles = perfiles;

            return View(persona);

        }

        [HttpPost]
        public async Task<IActionResult> Crear(PersonaCrearDTO personaCrearDTO)
        {

            var fotoURL = await almacenadorArchivos.Almacenar(contenedor, personaCrearDTO.Foto);

            var persona = new Persona
            {
                Nombre = personaCrearDTO.Nombre,
                FotoURL = fotoURL,
                FechaNacimiento = personaCrearDTO.FechaNacimiento
            };
            context.Add(persona);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> CrearPerfil(Guid id)
        {
            var persona = await context.Personas.FirstOrDefaultAsync(x => x.Id == id);

            if (persona is null)
            {
                return RedirectToAction("Index");
            }

            var perfilPersona = new PerfilPersona() { PartitionKey = id.ToString() };

            return View(perfilPersona);
        }

        [HttpPost]
        public async Task<IActionResult> CrearPerfil(PerfilPersona perfilPersona)
        {
            var id = Guid.Parse(perfilPersona.PartitionKey);
            var persona = await context.Personas.FirstOrDefaultAsync(x => x.Id == id);

            if (persona is null)
            {
                return RedirectToAction("Index");
            }

            perfilPersona.RowKey = Guid.NewGuid().ToString();
             
            await repositorioPerfiles.Crear(perfilPersona);
            return RedirectToAction("VerDetalle", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> ModificarPerfil(Guid id, string rowkey)
        {
            var persona = await context.Personas.FirstOrDefaultAsync(x => x.Id == id);

            if (persona is null)
            {
                return RedirectToAction("Index");
            }

            var perfil = await repositorioPerfiles.Obtener(id.ToString(), rowkey);

            if (perfil is null)
            {
                return RedirectToAction("Index");
            }

            perfil.ETagString = perfil.ETag.ToString();
            return View(perfil);
        }

        [HttpPost]
        public async Task<IActionResult> ModificarPerfil(PerfilPersona perfilPersona)
        {
            var id = Guid.Parse(perfilPersona.PartitionKey);
            var persona = await context.Personas.FirstOrDefaultAsync(x => x.Id == id);

            if (persona is null)
            {
                return RedirectToAction("Index");
            }

            perfilPersona.ETag = new ETag(perfilPersona.ETagString!);
            await repositorioPerfiles.Actualizar(perfilPersona);

            return RedirectToAction("VerDetalle", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> BorrarPerfil(Guid id, string rowkey)
        {
            var persona = await context.Personas.FirstOrDefaultAsync(x => x.Id == id);

            if (persona is null)
            {
                return RedirectToAction("Index");
            }

            var perfil = await repositorioPerfiles.Obtener(id.ToString(), rowkey);

            if (perfil is null)
            {
                return RedirectToAction("Index");
            }

            return View(perfil);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarPerfil(PerfilPersona perfilPersona)
        {
            var id = Guid.Parse(perfilPersona.PartitionKey);
            var persona = await context.Personas.FirstOrDefaultAsync(x => x.Id == id);

            if (persona is null)
            {
                return RedirectToAction("Index");
            }

            await repositorioPerfiles.Borrar(perfilPersona.PartitionKey, perfilPersona.RowKey);

            return RedirectToAction("VerDetalle", new { id });
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
