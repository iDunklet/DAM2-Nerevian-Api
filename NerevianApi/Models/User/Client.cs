using System.ComponentModel.DataAnnotations.Schema;

namespace NerevianApi.Models.User
{
    [Table("clients")]
    public class Client
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("usuari_id")]
        public int UserId { get; set; }

        // 👇 ESTA ES LA LÍNEA QUE FALTA PARA EL .ThenInclude(c => c.User)
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Column("dni_id")]
        public int? DniId { get; set; }

        [Column("actiu")]
        public bool Actiu { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}