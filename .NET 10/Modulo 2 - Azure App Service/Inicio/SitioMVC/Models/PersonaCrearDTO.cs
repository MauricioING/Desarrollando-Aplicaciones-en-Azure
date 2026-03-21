using System.ComponentModel.DataAnnotations;

namespace SitioMVC.Models
{
    public class PersonaCrearDTO
    {
        [Required]
        public required string Nombre { get; set; }
        public IFormFile Foto { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}
