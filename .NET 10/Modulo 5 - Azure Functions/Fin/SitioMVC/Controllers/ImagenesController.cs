using Comunes.Modelos;
using Microsoft.AspNetCore.Mvc;
using SitioMVC.Datos;
using SitioMVC.Models;
using SitioMVC.Servicios;

namespace SitioMVC.Controllers
{
    public class ImagenesController : Controller
    {
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly ApplicationDbContext context;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;
        private readonly string contenedor = "paralelo";

        public ImagenesController(IAlmacenadorArchivos almacenadorArchivos,
            ApplicationDbContext context, IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            this.almacenadorArchivos = almacenadorArchivos;
            this.context = context;
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
        }

        public IActionResult Index(string? mensaje = null)
        {
            if (mensaje is not null)
            {
                TempData["mensaje"] = mensaje;
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(EnviarImagenesViewModel modelo)
        {
            var tareas = new List<Task<string>>();
            foreach (var imagen in modelo.Imagenes)
            {
                tareas.Add(almacenadorArchivos.Almacenar(contenedor, imagen));
            }

            var urls = await Task.WhenAll(tareas);

            var reporte = new ReporteProcesoParaleloImagenes()
            {
                CorreoUsuario = modelo.Correo,
                Imagenes = urls.Select(url => new ImagenProcesoParalelo { Url = url }).ToList()
            };

            context.Add(reporte);
            await context.SaveChangesAsync();

            var httpClient = httpClientFactory.CreateClient();
            var llaveFuncion = configuration["LLAVE_OPTIMIZAR_GRUPO_IMAGENES"];
            var urlFuncion = configuration["URL_OPTIMIZAR_GRUPO_IMAGENES"];
            var urlConLlave = $"{urlFuncion}?code={llaveFuncion}";

            var jobId = reporte.Id;

            await httpClient.PostAsJsonAsync(urlConLlave, jobId);
            return RedirectToAction("Index", new { mensaje = "Proceso iniciado. Recibirá un correo cuando termine." });
        }

    }
}
