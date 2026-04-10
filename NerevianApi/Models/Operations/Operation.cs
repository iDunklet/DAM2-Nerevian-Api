using NerevianApi.Models.Operations;
using NerevianApi.Models.User;
using NerevianApi.Models.Business.Offer;

public class Operation
    {
        public int Id { get; set; }
        public Offer offer { get; set; }
        public string reference { get; set; }
        public OperationStatus status { get; set; }
        public Operation operation { get; set; }
        public Client client { get; set; }
        public DateTime InitialDate{ get; set; }
        public DateTime FinalDate { get; set; }
        public string[] observations { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

}

