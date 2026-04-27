using NerevianApi.Models.Operation;
using NerevianApi.Models.User;
using NerevianApi.Models.Documents;
using NerevianApi.Models.Business.Offer;
public class Document
    {
        public int id { get; set; }
        public string fileName { get; set; }
        public string originalName { get; set; }
        public DocumentType type { get; set; }
        public string path { get; set; }
        public string weight { get; set; }
        public Offer offer { get; set; }
        public Operation operation { get; set; }
        public User realesedBy { get; set; }
        public DateTime RealesedDate { get; set; }


}

