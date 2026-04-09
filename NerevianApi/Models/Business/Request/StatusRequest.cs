using System.ComponentModel.DataAnnotations.Schema;
namespace NerevianApi.Models.Business.Request
{
    [Table("estats_solicituds")]
    public class StatusRequest
    {
        public int Id { get; set; }
        [Column("estat")]
        public string status { get; set; }
    }
}
