using System.ComponentModel.DataAnnotations.Schema;

[Table("tipus_carrega")]
public class CargoType
{
    [Column("id")]
    public int Id { get; set; }

    [Column("tipus")] // En la BD se llama 'tipus'
    public string type { get; set; } = string.Empty;
}