using System.ComponentModel.DataAnnotations.Schema;
using NerevianApi.Models.User;
using NerevianApi.Models.Business.Request;

namespace NerevianApi.Models.Business.Offer
{
    [Table("oferta")]
    public class Offer
    {
        // --- Campos Escalares ---

        [Column("id")]
        public int id { get; set; }

        [Column("data_validacio_inicial")]
        public DateTime initialValidationDate { get; set; }

        [Column("data_validacio_final")]
        public DateTime finalValidationDate { get; set; }

        [Column("moneda")]
        public string coin { get; set; }

        [Column("pressupost")]
        public string budget { get; set; }

        [Column("comentaris")]
        public string comments { get; set; }

        [Column("motiu_rebuig")]
        public string denyReason { get; set; }

        // --- Claves Foráneas (FK) y Propiedades de Navegación ---

        // Relación con Estado
        [Column("estat_oferta_id")]
        public int estat_oferta_id { get; set; }

        [ForeignKey("estat_oferta_id")]
        public StatusOffer status { get; set; }

        // Relación con Cliente
        [Column("client_id")]
        public int client_id { get; set; }

        [ForeignKey("client_id")]
        public Client client { get; set; }

        // Relación con Solicitud (Request)
        // Nota: Especificamos Request.Request por el namespace, para evitar ambigüedades
        [Column("solicitud_id")]
        public int solicitud_id { get; set; }

        [ForeignKey("solicitud_id")]
        public Request.Request request { get; set; }
    }
}