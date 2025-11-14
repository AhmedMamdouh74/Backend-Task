using Application.DTOs;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IIpService
    {

        Task<IpLookupResponseDto> LookupAsync(string ipAddress);
        Task<IpCheckResponseDto> CheckBlockAsync(string ipAddress, string userAgent);
        Task<PagedResult<BlockLog>> GetLogsAsync(int page, int pageSize);



    }
}
