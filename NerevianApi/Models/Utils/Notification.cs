using System.ComponentModel.DataAnnotations.Schema;
using NerevianApi.Models.Incoterms;
using NerevianApi.Models.Business.Request;

namespace NerevianApi.Models.Utils
{
    [Table("notification")]
    public class Notification
    {
        [Column("id")]
        public int Id { get; set; }

        // Según tu script: FK_notification_Incoterm usa [incoterm_id]
        [Column("incoterm_id")]
        public int IncotermId { get; set; }

        [ForeignKey("IncotermId")]
        public Incoterm incoterm { get; set; } // Ojo: esto apunta a la tabla 'Incoterm', no a 'Incoterm_type'

        [Column("solicitud_id")]
        public int RequestId { get; set; }

        [ForeignKey("RequestId")]
        public Request request { get; set; }

        // Según tu script: [date_update]
        [Column("date_update")]
        public DateTime updateDate { get; set; }
    }
}