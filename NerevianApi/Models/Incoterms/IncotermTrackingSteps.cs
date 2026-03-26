namespace NerevianApi.Models.Incoterms
{
    public class IncotermTrackingSteps
    {
        public int Id { get; set; }
        public IncotermType incotermType { get; set; }
        public TrackingSteps trackingSteps { get; set; }
    }
}
