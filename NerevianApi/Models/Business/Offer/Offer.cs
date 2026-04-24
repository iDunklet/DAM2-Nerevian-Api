using System.ComponentModel.DataAnnotations.Schema;
using NerevianApi.Models.User;
// Asegúrate de tener los using correctos para StatusOffer y Request

namespace NerevianApi.Models.Business.Offer
{
    [Table("ofertes")] // <-- CORREGIDO: Ahora coincide con SQL
    public class Offer
    {
        [Column("id")]
        public int id { get; set; }

        [Column("data_validessa_inicial")] // <-- CORREGIDO: 'validessa' con doble S
        public DateTime initialValidationDate { get; set; }

        [Column("data_validessa_final")] // <-- CORREGIDO
        public DateTime finalValidationDate { get; set; }

        [Column("moneda")]
        public string coin { get; set; } = string.Empty;

        [Column("pressupost")]
        public double budget { get; set; } // <-- CORREGIDO: De string a double (por el float de SQL)

        [Column("comentaris")]
        public string? comments { get; set; }

        [Column("deny_reason")] // <-- CORREGIDO: En SQL lo tienes en inglés
        public string? denyReason { get; set; }

        // --- Claves Foráneas ---

        [Column("estat_oferta_id")]
        public int estat_oferta_id { get; set; }

        [ForeignKey("estat_oferta_id")]
        public StatusOffer? status { get; set; }

        [Column("clients_id")] // <-- CORREGIDO: En SQL está en plural
        public int client_id { get; set; }

        [ForeignKey("client_id")]
        public Client? client { get; set; }

        [Column("solicitud_id")]
        public int solicitud_id { get; set; }

        [ForeignKey("solicitud_id")]
        public NerevianApi.Models.Business.Request.Request? request { get; set; }
    }
}