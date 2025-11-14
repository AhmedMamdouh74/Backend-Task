using Application.Services;

namespace Infrastructure.Service
{
    public class GeoLocationService : IGeoLocationService
    {
        private readonly HttpClient httpClient;
        public GeoLocationService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }
        public async Task<(string CountryCode, string CountryName, string ISP)> GetCountryByIpAsync(string ipAddress)
        {

            var url = $"https://ipapi.co/{ipAddress}/json/";
            var response = await httpClient.GetStringAsync(url);
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
            return (data.country_code, data.country_name, data.org);

        }
    }
}
