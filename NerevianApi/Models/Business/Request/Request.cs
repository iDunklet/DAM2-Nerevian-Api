using NerevianApi.Models.Incoterms;
using NerevianApi.Models.Logistics;
using NerevianApi.Models.Operations;
using NerevianApi.Models.User;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;

namespace NerevianApi.Models.Business.Request
{
    // Le decimos a Entity Framework que busque la tabla "solicitud" en SQL
    [Table("solicitud")]
    public class Request
    {
        // Clave primaria
        [Column("id")]
        public int Id { get; set; }

        // --- Gestión del Estado ---
        // El ID real numérico que se guarda en la base de datos
        [Column("estat_solicitud_id")]
        public int estat_solicitud_id { get; set; }

        // El objeto de navegación (EF Core lo rellena usando el ID de arriba)
        [ForeignKey("estat_solicitud_id")]
        public StatusRequest status { get; set; }

        // --- Campos básicos mapeados ---
        // Le indicamos el nombre en catalán para que no pete al hacer selects
        [Column("comentaris")]
        public string comments { get; set; }

        [Column("data_creacio")]
        public DateTime createdAt { get; set; }

        // --- Objetos no mapeados en base de datos ---
        // Con [NotMapped] evitamos que EF Core intente buscar estas columnas y dé error
        [NotMapped] public TransportType transportType { get; set; }
        [NotMapped] public FlowType flowType { get; set; }
        [NotMapped] public CargoType cargoType { get; set; }
        [NotMapped] public IncotermType incotermType { get; set; }
        [NotMapped] public Client client { get; set; }
        [NotMapped] public Carrier carrier { get; set; }
        [NotMapped] public string rawWeight { get; set; }
        [NotMapped] public string rawVolume { get; set; }
        [NotMapped] public ValidationType validationType { get; set; }
        [NotMapped] public Port originPort { get; set; }
        [NotMapped] public Port destinationPort { get; set; }
        [NotMapped] public Operation operation { get; set; }
        [NotMapped] public ContainerType containerType { get; set; }
    }
}