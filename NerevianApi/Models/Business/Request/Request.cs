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

        // --- Relaciones para el Endpoint ---
        // IMPORTANTE: Quitamos [NotMapped] de los puertos y carga para que el Endpoint pueda traer los nombres

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

        // --- Objetos con conflicto de nombre ---
        // Usamos la ruta completa para que C# no diga que "Operation es un namespace"

        [NotMapped]
        public NerevianApi.Models.Operation.Operation operation { get; set; }

        // --- Otros campos NotMapped ---
        [NotMapped] public TransportType transportType { get; set; }
        [NotMapped] public FlowType flowType { get; set; }
        [NotMapped] public IncotermType incotermType { get; set; }
        [NotMapped] public Client client { get; set; }
        [NotMapped] public Carrier carrier { get; set; }
        [NotMapped] public string rawWeight { get; set; }
        [NotMapped] public string rawVolume { get; set; }
        [NotMapped] public ValidationType validationType { get; set; }
        [NotMapped] public ContainerType containerType { get; set; }
    }
}