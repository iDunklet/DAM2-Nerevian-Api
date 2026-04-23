// NerevianApi/Models/User/User.cs
using System.ComponentModel.DataAnnotations.Schema;

namespace NerevianApi.Models.User
{
    [Table("usuaris")]  // Nombre de la tabla en la base de datos
    public class User
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("correu")]  // En BD se llama 'correu', no 'email'
        public string email { get; set; } = string.Empty;

        [Column("contrasenya")]  // En BD se llama 'contrasenya', no 'Password'
        public string Password { get; set; } = string.Empty;

        [Column("nom")]  // En BD se llama 'nom', no 'Name'
        public string Name { get; set; } = string.Empty;

        [Column("cognoms")]  // En BD se llama 'cognoms', no 'Surname'
        public string Surname { get; set; } = string.Empty;

        [Column("telefon")]  // En BD se llama 'telefon', no 'PhoneNumber'
        public string PhoneNumber { get; set; } = string.Empty;

        [Column("rol_id")]  // En BD se llama 'rol_id', no 'Roleid'
        public int Roleid { get; set; } = 1;

        // Campos opcionales que tiene la tabla
        [Column("created_at")]
        public DateTime? created_at { get; set; }

        [Column("updated_at")]
        public DateTime? updated_at { get; set; }

        [Column("deleted_at")]
        public DateTime? deleted_at { get; set; }
    }
}