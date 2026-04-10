using System.ComponentModel.DataAnnotations.Schema;

namespace NerevianApi.Models.Business.Offer
{
    [Table("estat_oferta")]
    public class StatusOffer
    {
        [Column("id")]
        public int id { get; set; }

        [Column("estat")]
        public string status { get; set; }
    }
}