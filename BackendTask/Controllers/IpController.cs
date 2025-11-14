using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/ip")]
    public class IpController : ControllerBase
    {
        private readonly IIpService ipService;

        public IpController(IIpService ipService)
        {
            this.ipService = ipService;
        }

        [HttpGet("lookup")]
        public async Task<IActionResult> Lookup([FromQuery] string? ipAddress)
        {
            
            ipAddress ??= HttpContext.Connection.RemoteIpAddress?.ToString();

            
            if (!IPAddress.TryParse(ipAddress, out _))
            {
               
                return BadRequest("Invalid IP address format.");
            }
           
            var result = await ipService.LookupAsync(ipAddress);
            return Ok(result);
        }

        [HttpGet("check-block")]
        public async Task<IActionResult> CheckBlock()
        {
   
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
           

           
            if (string.IsNullOrWhiteSpace(ip) || ip == "::1" || ip == "127.0.0.1")
            {
                ip = "8.8.8.8"; // Use a known public IP for testing
            }


            if (!IPAddress.TryParse(ip, out _))
            {
               
                return BadRequest("Unable to resolve client IP.");
            }



            var userAgent = Request.Headers["User-Agent"].ToString();
            var result = await ipService.CheckBlockAsync(ip, userAgent);
            return Ok(result);
        }

        [HttpGet("/api/logs/blocked-attempts")]
        public async Task<IActionResult> GetLogs([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await ipService.GetLogsAsync(page, pageSize);
            return Ok(result);
        }
    }
}
