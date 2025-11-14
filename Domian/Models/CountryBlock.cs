namespace Models
{
    public class CountryBlock
    {
        public string CountryCode { get; set; } = "";
        public string CountryName { get; set; } = "";
        public DateTime? BlockedUntilUtc { get; set; } 
        public bool Blocked { get; set; }
        public bool IsTemporal => BlockedUntilUtc.HasValue;
    }
}
