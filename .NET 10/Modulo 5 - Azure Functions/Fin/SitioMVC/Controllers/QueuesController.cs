using Comunes.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SitioMVC.Datos;
using SitioMVC.Servicios;
using System.Text.Json;

namespace SitioMVC.Controllers
{
    public class QueuesController : Controller
    {
        private readonly IQueueServicio queueServicio;
        private readonly ApplicationDbContext context;
        private readonly string queue = "ejemplo";

        public QueuesController(IQueueServicio queueServicio, ApplicationDbContext context)
        {
            this.queueServicio = queueServicio;
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string mensaje)
        {
            await queueServicio.EscribirMensaje(queue, mensaje);
            return RedirectToAction("Index");
        }

        public IActionResult GenerarReporte()
        {
            return View();
        }

        [HttpPost]
        public async Task<Reporte> GenerarReporte([FromBody] string correoUsuario)
        {
            Console.WriteLine("generar reporte");
            var reporte = new Reporte { CorreoUsuario = correoUsuario };
            context.Add(reporte);
            await context.SaveChangesAsync();

            var mensajeQueueObj = new RealizarReporteMensajeDTO
            {
                ReporteId = reporte.Id,
                CorreoUsuario = reporte.CorreoUsuario
            };

            var json = JsonSerializer.Serialize(mensajeQueueObj);
            await queueServicio.EscribirMensaje("reportes-personas", json);
            return reporte;
        }

        [HttpGet("[controller]/api/{id}")]
        public async Task<Reporte> ObtenerEstatusReporte(Guid id)
        {
            return await context.Reportes.FirstAsync(x => x.Id == id);
        }


    }
}
