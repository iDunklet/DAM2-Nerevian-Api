using System.ComponentModel.DataAnnotations.Schema;

namespace NerevianApi.Models.Logistics
{
    [Table("ports")] 
    public class Port
    {
        [Column("id")]
        public int Id { get; set; } 

        [Column("nom")] 
        public string Name { get; set; }

        [Column("ciutat_id")] 
        public int CityId { get; set; }

    }
}
