using NerevianApi.Models.Incoterms;
using NerevianApi.Models.Business.Request;

namespace NerevianApi.Models.Utils
{
    public class Notification
    {
        public int Id { get; set; }
        public IncotermType incotermType { get; set; }
        public Request request { get; set; }
        public DateTime updateDate { get; set; }

    }
}
