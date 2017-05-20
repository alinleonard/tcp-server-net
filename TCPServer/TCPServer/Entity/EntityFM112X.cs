namespace TCPServer.Entity
{
    public class EntityFM112X
    {
        public string IMEI { get; set; }

        public string CodecId { get; set; }
        public long TimeStamp { get; set; }
        public int Priority { get; set; }

        public long Longitude { get; set; }
        public long Latitude { get; set; }
        public int Altitude { get; set; }
        public int Angle { get; set; }
        public int VisibleSattelites { get; set; }
        public int Speed { get; set; }
    }
}
