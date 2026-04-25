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

        [Column("incoterm_type_id")] // Ajusta este nombre si en tu DB es diferente
        public int? IncotermTypeId { get; set; }

        [ForeignKey("IncotermTypeId")]
        public IncotermType incotermType { get; set; }

        [Column("solicitud_id")] // Ajusta este nombre según tu DB
        public int? RequestId { get; set; }

        [ForeignKey("RequestId")]
        public Request request { get; set; }

        [Column("data_actualitzacio")] // Ajusta según el nombre real en tu SQL (ej. update_date o data_update)
        public DateTime updateDate { get; set; }
    }
}