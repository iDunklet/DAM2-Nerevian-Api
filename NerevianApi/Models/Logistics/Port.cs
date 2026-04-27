using NerevianApi.Models.Logistics;
using System.ComponentModel.DataAnnotations.Schema;

[Table("ports")]
public class Port
{
    [Column("id")]
    public int id { get; set; }

    [Column("nom")] // En la BD se llama 'nom'
    public string name { get; set; } = string.Empty;

    [Column("ciutat_id")]
    public int cityId { get; set; }

    [NotMapped] // Si no tienes el modelo City mapeado aún, déjalo como NotMapped
    public City? city { get; set; }
}