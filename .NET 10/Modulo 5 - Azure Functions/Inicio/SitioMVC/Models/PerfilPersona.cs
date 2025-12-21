using Azure;
using Azure.Data.Tables;
using System.Runtime.Serialization;

namespace SitioMVC.Models
{
    public class PerfilPersona : ITableEntity
    {
        // Id de la persona
        public string PartitionKey { get; set; }
        // Id del perfil
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        [IgnoreDataMember]
        public string? ETagString { get; set; }

        public string Idioma { get; set; } = "ESP";
        public bool ModoOscuro { get; set; } = false;
    }
}
