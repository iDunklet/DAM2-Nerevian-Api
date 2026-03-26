using NerevianApi.Models.Incoterms;
using NerevianApi.Models.Logistics;
using NerevianApi.Models.Operations;
using NerevianApi.Models.User;
using System.Xml;

namespace NerevianApi.Models.Business.Request
{
    public class Request
    {
        public int Id { get; set; }
        public TransportType transportType { get; set; }
        public FlowType flowType { get; set; }
        public CargoType cargoType { get; set; }
        public IncotermType incotermType { get; set; }
        public Client client { get; set; }
        public string[] comments { get; set; }
        public Carrier carrier { get; set; }
        public String rawWeight { get; set; }
        public String rawVolume { get; set; }
        public ValidationType validationType { get; set; }
        public Port originPort { get; set; }
        public Port destinationPort { get; set; }
        public StatusRequest status { get; set; }
        public Operation operation { get; set; }
        public DateTime createdAt { get; set; }
        public ContainerType containerType { get; set; }

    }
}
