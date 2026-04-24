using NerevianApi.Models.Business.Offer;
using NerevianApi.Models.User;
using System.ComponentModel.DataAnnotations.Schema;
// Quitamos el using conflictivo y usamos la ruta completa abajo

namespace NerevianApi.Models.Operation
{
    [Table("operacions")]
    public class Operation
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("oferta_id")]
        public int OfferId { get; set; }

        [ForeignKey("OfferId")]
        public Offer Offer { get; set; }

        [Column("codi_referencia")]
        public string Reference { get; set; } = string.Empty;

        [Column("estat_id")]
        public int StatusId { get; set; }

        [ForeignKey("StatusId")]
        public OperationStatus Status { get; set; }

        [Column("operador_id")]
        public int OperatorId { get; set; }

        [ForeignKey("OperatorId")]
        public NerevianApi.Models.User.User Operator { get; set; } // Solución al error

        [Column("client_id")]
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; } // Verifica que Client no tenga el mismo problema

        [Column("data_inici")]
        public DateTime? InitialDate { get; set; }

        [Column("data_finalitzacio")]
        public DateTime? FinalDate { get; set; }

        [Column("observacions")]
        public string? Observations { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}