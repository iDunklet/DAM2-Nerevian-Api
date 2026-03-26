using NerevianApi.Models.Business.Offer;
using NerevianApi.Models.User;
using NerevianApi.Models.Business.Request;


public class Offer
    {
        public int id { get; set; }
        public DateTime initialValidationDate { get; set; }
        public DateTime finalValidationDate { get; set; }
        public string coin { get; set; }
        public Client client { get; set; }
        public string budget { get; set; }
        public string[] comments { get; set; }
        public StatusOffer status { get; set; }
        public Request request { get; set; }
        public string denyReason { get; set; }
}

