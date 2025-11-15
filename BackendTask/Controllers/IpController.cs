using Api.Models;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/ip")]
    public class IpController : ControllerBase
    {
        private readonly IIpService ipService;
        private readonly IGeoLocationService geoLocationService;
        private readonly IConfiguration config;

        public IpController(IIpService _ipService,IGeoLocationService _geoLocationService,IConfiguration _config)
        {
            ipService = _ipService;
            geoLocationService = _geoLocationService;
            config= _config;
        }

       
        [HttpGet("lookup")]
        public async Task<IActionResult> Lookup([FromQuery] string? ipAddress)
        {
            try
            {
                // 1. If no IP is provided, use caller IP from HttpContext
                if (string.IsNullOrWhiteSpace(ipAddress))
                {
                    ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                    if (ipAddress == "::1")
                    {
                        ipAddress = config["DevFallbackIp"]; // fallback for local dev only
                    }
                }

                // 2. Validate IP format
                if (!IPAddress.TryParse(ipAddress, out _))
                {
                    return BadRequest(ApiResponse.Fail($"Invalid IP address: {ipAddress}"));
                }
              



                var result = await geoLocationService.GetCountryByIpAsync(ipAddress);

                return Ok(ApiResponse.Ok("IP lookup completed.", new
                {
                    ipAddress,
                    result.CountryCode,
                    result.CountryName,
                    result.ISP
                }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Fail("Internal server error.", ex.Message));
            }
        }


        [HttpGet("check-block")]

        public async Task<IActionResult> CheckBlock()
        {
            try
            {
                // 1. Fetch caller external IP from HttpContext
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
                // to avoid ::1 in development environment
                if (ip == "::1")
                {
                    ip = config["DevFallbackIp"]; 
                }



              
                if (!IPAddress.TryParse(ip, out _))
                    return BadRequest(ApiResponse.Fail("Unable to resolve client IP."));

               
                var userAgent = Request.Headers["User-Agent"].ToString();

                
                var result = await ipService.CheckBlockAsync(ip, userAgent);

                return Ok(ApiResponse.Ok("IP block check completed.", result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Fail("Internal server error.", ex.Message));
            }
        }


        [HttpGet("/api/logs/blocked-attempts")]
        public async Task<IActionResult> GetLogs(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await ipService.GetLogsAsync(page, pageSize);
                return Ok(ApiResponse.Ok("Blocked attempts retrieved successfully.", result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Fail("Failed to fetch logs.", ex.Message));
            }
        }
    }
}
