using Comunes.Modelos;
using Microsoft.EntityFrameworkCore;
using SitioMVC.Models;

namespace SitioMVC.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected ApplicationDbContext()
        {
        }

        public DbSet<Persona> Personas { get; set; }
        public DbSet<Reporte> Reportes { get; set; }
        public DbSet<ReporteProcesoParaleloImagenes> ReportesProcesoParaleloImagenes { get; set; }
    }
}
