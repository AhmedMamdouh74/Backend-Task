using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;


namespace Test_Assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryBlockService countryBlockService;
        public CountriesController(ICountryBlockService _countryBlockService)
        {
            countryBlockService = _countryBlockService;
        }
        [HttpPost("block")]
        public async Task<IActionResult> BlockCountry([FromBody] AddBlockedCountryDto blockDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await countryBlockService.AddBlockAsync(blockDto);
            if (!result)
            {
                return Conflict("Country is already blocked or invalid country code.");
            }
            return Ok("Country blocked successfully.");
        }
        [HttpDelete("block/{CountryCode}")]
        public async Task<IActionResult> RemoveBlockCountry([FromRoute] RemoveBlockedCountryDto removedCountryDto)
        {
            var ok = await countryBlockService.RemoveBlockAsync(removedCountryDto);
            if (!ok) return NotFound();
            return NoContent();
        }
        [HttpGet("blocked")]
        public async Task<IActionResult> GetBlockedCountries([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            var result = await countryBlockService.GetAllAsync(page, pageSize, search);
            return Ok(result);
        }
        [HttpPost("temporal-block")]
        public async Task<IActionResult> AddTemporalBlock([FromBody] TemporalBlockDto temporalBlockDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await countryBlockService.AddTemporalBlockAsync(temporalBlockDto);
            if (!result)
            {
                return Conflict("Country is already blocked or invalid country code.");
            }
            return Ok("Country temporally blocked successfully.");
        }
    }
}
