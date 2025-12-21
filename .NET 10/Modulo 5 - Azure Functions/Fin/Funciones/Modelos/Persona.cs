using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Funciones.Modelos
{
    public class Persona
    {
        public Guid Id { get; set; }
        public required string Nombre { get; set; }
        public required string FotoURL { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? Correo { get; set; }
    }
}
