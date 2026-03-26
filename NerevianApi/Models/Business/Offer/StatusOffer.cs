namespace NerevianApi.Models.Business.Offer
{
 
    public enum StatusOffer
    {
        Pending,
        Accepted,
        Rejected

        public int id { get; set; }
        public string status { get; set; }

    }
}