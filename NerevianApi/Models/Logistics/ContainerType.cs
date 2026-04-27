using System.ComponentModel.DataAnnotations.Schema;

namespace NerevianApi.Models.Logistics
{
    [Table("tipus_contenidors")] // Nombre real de la tabla en tu SQL
    public class ContainerType
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("tipus")] // Nombre real de la columna en tu SQL
        public string Name { get; set; } = string.Empty;
    }
}