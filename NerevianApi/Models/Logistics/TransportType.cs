using System.ComponentModel.DataAnnotations.Schema;

namespace NerevianApi.Models.Logistics
{
    [Table("tipus_transports")]
    public class TransportType
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("tipus")]
        public string type { get; set; } = string.Empty;
    }
}
