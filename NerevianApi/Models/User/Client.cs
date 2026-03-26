namespace NerevianApi.Models.User
{
    public class Client
    {
        public int Id { get; set; }
        public User user { get; set; }
        public DNI dni { get; set; }
        public DateTime registeDate { get; set; }
    }
}
