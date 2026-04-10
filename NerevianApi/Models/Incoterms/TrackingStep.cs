using System.ComponentModel.DataAnnotations.Schema;

[Table("tracking_steps")]
public class TrackingStep
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")] // En tu script es 'name'
    public string Name { get; set; }
}