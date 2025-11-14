using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IGeoLocationService
    {
        Task<(string CountryCode, string CountryName,string ISP)> GetCountryByIpAsync(string ipAddress);
    }
}
