using NerevianApi.Models.Operations;
using NerevianApi.Models.User;
using NerevianApi.Models.Documents;
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

