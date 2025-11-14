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

        public IpController(IIpService ipService)
        {
            this.ipService = ipService;
        }

        
        [HttpGet("lookup")]
        public async Task<IActionResult> Lookup([FromQuery] string? ipAddress)
        {
            try
            {
                // 1. Use caller IP if none provided
                ipAddress ??= HttpContext.Connection.RemoteIpAddress?.ToString();

                if (string.IsNullOrWhiteSpace(ipAddress))
                    return BadRequest(ApiResponse.Fail("Could not identify client IP address."));

                // 2. Validate IP format
                if (!IPAddress.TryParse(ipAddress, out _))
                    return BadRequest(ApiResponse.Fail("Invalid IP address format."));

                // 3. Call the Application Layer
                var result = await ipService.LookupAsync(ipAddress);

                return Ok(ApiResponse.Ok("IP lookup successful.", result));
            }
            catch (Exception ex)
            {
                return StatusCode(502, ApiResponse.Fail("Geo lookup failed.", ex.Message));
            }
        }

     
        [HttpGet("check-block")]
        public async Task<IActionResult> CheckBlock()
        {
            try
            {
                // 1. Fetch caller external IP from HttpContext
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

                // Handle localhost development scenario
                if (string.IsNullOrWhiteSpace(ip) || ip == "::1" || ip == "127.0.0.1")
                    ip = "8.8.8.8"; // fallback for testing

                // 2. Validate the IP
                if (!IPAddress.TryParse(ip, out _))
                    return BadRequest(ApiResponse.Fail("Unable to resolve client IP."));

                // 3. Get User-Agent
                var userAgent = Request.Headers["User-Agent"].ToString();

                // 4. Perform block check
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
