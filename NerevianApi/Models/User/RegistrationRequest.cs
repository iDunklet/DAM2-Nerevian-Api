namespace NerevianApi.Models.User
{
    public class RegistrationRequest
    {
        public int id { get; set; }
        public string companyName { get; set; }
        public string contact { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string receivedReason { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime resolutionDate { get; set; }
        public User resolvedBy { get; set; }
        public string password { get; set; }
    }
}
