using System.ComponentModel.DataAnnotations;

namespace SitioMVC.Models
{
    public class EnviarImagenesViewModel
    {
        [Required]
        [EmailAddress]
        public required string Correo { get; set; }

        [Required(ErrorMessage = "Debes seleccionar al menos una imagen.")]
        public List<IFormFile> Imagenes { get; set; } = [];
    }
}
