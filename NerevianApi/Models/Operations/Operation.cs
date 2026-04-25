using System.ComponentModel.DataAnnotations.Schema;
using NerevianApi.Models.User;
using NerevianApi.Models.Business.Offer;

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
        public Offer offer { get; set; }

        [Column("codi_referencia")]
        public string reference { get; set; } = string.Empty;

        [Column("estat_id")]
        public int? StatusId { get; set; }
        [ForeignKey("StatusId")]
        public OperationStatus status { get; set; }

        [Column("operador_id")]
        public int OperatorId { get; set; }
        // Aquí asumiríamos que tienes un User para el Operador

        [Column("client_id")]
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client client { get; set; }

        [Column("data_inici")]
        public DateTime? InitialDate { get; set; }

        [Column("data_finalitzacio")]
        public DateTime? FinalDate { get; set; }

        [Column("observacions")]
        public string? observations { get; set; } // En BD es varchar(255), no un array[]

        [Column("created_at")]
        public DateTime? createdAt { get; set; }

        [Column("updated_at")]
        public DateTime? updatedAt { get; set; }
    }
}