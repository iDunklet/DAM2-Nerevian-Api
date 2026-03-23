namespace NerevianApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // En el futuro la encriptaremos
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int Roleid { get; set; } = 1; // Por defecto user
    }
}
