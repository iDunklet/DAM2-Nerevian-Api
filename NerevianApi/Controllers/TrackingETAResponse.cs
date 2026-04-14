namespace NerevianApi.Models 
{

    public class TrackingStatusItem
    {
        public string Title { get; set; }
        public string Time { get; set; }
        public bool IsCompleted { get; set; }
    }
    public class TrackingETAResponse
    {
        public string ReferenceCode { get; set; }
        public string Route { get; set; }
        public string EtaDate { get; set; }
        public string GlobalStatus { get; set; }
        public string ContainerNumber { get; set; }

        public List<TrackingStatusItem> History { get; set; }
    }

}