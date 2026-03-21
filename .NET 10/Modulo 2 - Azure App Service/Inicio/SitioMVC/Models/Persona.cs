namespace SitioMVC.Models
{
    public class Persona
    {
        public Guid Id { get; set; }
        public required string Nombre { get; set; }
        public required string FotoUrl { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}
