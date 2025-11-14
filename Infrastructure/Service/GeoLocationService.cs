using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Infrastructure.Service
{
    public class GeoLocationService : IGeoLocationService
    {
        private readonly HttpClient httpClient;
        private readonly string apiKey;
        private readonly ILogger<GeoLocationService> logger;
        public GeoLocationService(HttpClient _httpClient, IConfiguration config, ILogger<GeoLocationService> _logger)
        {
            httpClient = _httpClient;
            apiKey = config["GeoLocation:ApiKey"];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException("GeoLocation:ApiKey is missing from configuration.");
            }

            logger = _logger;
        }
        public async Task<(string CountryCode, string CountryName, string ISP)> GetCountryByIpAsync(string ipAddress)
        {
           

            var url = $"https://api.ipgeolocation.io/ipgeo?apiKey={apiKey}&ip={ipAddress}";
            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Geo API failed for IP {IpAddress}. Status: {StatusCode}", ipAddress, response.StatusCode);
                return ("", "", "Unknown ISP");
            }
            var responseBody = await response.Content.ReadAsStringAsync();

            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);

            string countryCode = data.country_code2;
            string countryName = data.country_name;


            string isp = data.isp ?? data.organization ?? data.network?.asn?.asn_name ?? "Unknown ISP";

            return (countryCode, countryName, isp);
        }

    }
}
