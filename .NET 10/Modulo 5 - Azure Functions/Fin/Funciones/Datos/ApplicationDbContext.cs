using Comunes.Modelos;
using Funciones.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Funciones.Datos
{
    public class ApplicationDbContext: DbContext
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
