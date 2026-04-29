using System.ComponentModel.DataAnnotations.Schema;
using NerevianApi.Models.Incoterms;

namespace NerevianApi.Models.Incoterms
{
    [Table("Incoterm")] 
    public class Incoterm
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("incoterm_type_id")]
        public int IncotermTypeId { get; set; }

        [Column("tracking_step_id")]
        public int TrackingStepId { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Relaciones (Navigation Properties)
        [ForeignKey("IncotermTypeId")]
        public virtual IncotermType IncotermType { get; set; }

        [ForeignKey("TrackingStepId")]
        public virtual TrackingStep TrackingStep { get; set; }
    }
}
