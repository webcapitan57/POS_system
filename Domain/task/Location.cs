namespace POC.BL.Domain.task
{
    public class Location
    {
        public int LocationId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Radius { get; set; }
        public PhotoQuestion PhotoQuestion { get; set; }
    }
}