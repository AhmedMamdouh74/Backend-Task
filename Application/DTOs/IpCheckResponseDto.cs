namespace Application.DTOs
{
    public record IpCheckResponseDto(string IpAddress, string CountryCode, bool IsBlocked) { }
   
}
