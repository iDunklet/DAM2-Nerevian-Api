using System.ComponentModel.DataAnnotations.Schema;

[Table("estats_operacions")]
public class OperationStatus
{
    [Column("id")]
    public int Id { get; set; }

    [Column("estat")] // En la BD se llama 'estat'
    public string status { get; set; } = string.Empty;
}