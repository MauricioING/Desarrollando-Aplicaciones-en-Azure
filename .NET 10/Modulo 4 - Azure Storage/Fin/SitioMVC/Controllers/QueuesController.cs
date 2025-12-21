using Microsoft.AspNetCore.Mvc;
using SitioMVC.Servicios;

namespace SitioMVC.Controllers
{
    public class QueuesController : Controller
    {
        private readonly IQueueServicio queueServicio;

        private readonly string queue = "ejemplo";

        public QueuesController(IQueueServicio queueServicio)
        {
            this.queueServicio = queueServicio;
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
    }
}
