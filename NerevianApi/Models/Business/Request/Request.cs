using NerevianApi.Models.Incoterms;
using NerevianApi.Models.Logistics;
using NerevianApi.Models.User;
using System.ComponentModel.DataAnnotations.Schema;
using NerevianApi.Models.Utils;

namespace NerevianApi.Models.Business.Request
{
    [Table("solicitud")]
    public class Request
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("estat_solicitud_id")]
        public int estat_solicitud_id { get; set; }

        [ForeignKey("estat_solicitud_id")]
        public StatusRequest status { get; set; }

        [Column("comentaris")]
        public string comments { get; set; } = string.Empty;

        [Column("data_creacio")]
        public DateTime createdAt { get; set; }

        [Column("updated_at")]
        public DateTime? updatedAt { get; set; }

        [Column("pes_brut")]
        public decimal? pes_brut { get; set; }

        [Column("volum")]
        public decimal? volum { get; set; }

        // --- Relaciones de Logística ---

        [Column("port_origen_id")]
        public int? originPortId { get; set; }
        [ForeignKey("originPortId")]
        public Port originPort { get; set; }

        [Column("port_desti_id")]
        public int? destinationPortId { get; set; }
        [ForeignKey("destinationPortId")]
        public Port destinationPort { get; set; }

        [Column("tipus_carrega_id")]
        public int? cargoTypeId { get; set; }
        [ForeignKey("cargoTypeId")]
        public CargoType cargoType { get; set; }

        // IMPORTANTE: Quitamos NotMapped para que funcione el Tracking
        [Column("tipus_contenidor_id")]
        public int? containerTypeId { get; set; }
        [ForeignKey("containerTypeId")]
        public ContainerType containerType { get; set; }

        [Column("tipus_transport_id")]
        public int transportTypeId { get; set; }
        [ForeignKey("transportTypeId")]
        public TransportType transportType { get; set; }

        // --- Historial y Notificaciones ---

        // Esta es la colección que alimenta la línea de tiempo en Android
        public ICollection<Notification> notifications { get; set; }

        // --- Campos que se mantienen NotMapped (Cosas de lógica de negocio pura) ---

        [NotMapped]
        public NerevianApi.Models.Operation.Operation operation { get; set; }

        [NotMapped] public FlowType flowType { get; set; }
        [NotMapped] public IncotermType incotermType { get; set; }
        [NotMapped] public Client client { get; set; }
        [NotMapped] public Carrier carrier { get; set; }
        [NotMapped] public ValidationType validationType { get; set; }
    }
}