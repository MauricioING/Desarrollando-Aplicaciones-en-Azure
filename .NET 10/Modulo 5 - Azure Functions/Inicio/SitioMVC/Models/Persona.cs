using System.ComponentModel.DataAnnotations.Schema;

namespace SitioMVC.Models
{
    public class Persona
    {
        public Guid Id { get; set; }
        public required string Nombre { get; set; }
        public required string FotoURL { get; set; }
        public DateTime FechaNacimiento { get; set; }
        [NotMapped]
        public List<PerfilPersona>? Perfiles { get; set; }
    }
}
