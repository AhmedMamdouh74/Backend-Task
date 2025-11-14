namespace Models
{
    public class BlockLog
    {
        public string Ip { get; set; } = "";
        public DateTime TimestampUtc { get; set; }
        public string CountryCode { get; set; } = "";
        public bool Blocked { get; set; }
        public string UserAgent { get; set; } = "";

    }
}
