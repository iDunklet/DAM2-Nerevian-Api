using System.ComponentModel.DataAnnotations.Schema;

namespace NerevianApi.Models.Operation
{
    [Table("estats_operacions")]
    public class OperationStatus
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("estat")]
        public string status { get; set; } = string.Empty;
    }
}