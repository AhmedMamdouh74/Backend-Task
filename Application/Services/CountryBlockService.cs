using Application.DTOs;
using Domain.Repos;
using Microsoft.Extensions.Logging;
using Models;

namespace Application.Services
{
    public class CountryBlockService : ICountryBlockService
    {
        private readonly ICountryRepo repo;
        private readonly ILogger<CountryBlockService> logger;
        public CountryBlockService(ICountryRepo _repo, ILogger<CountryBlockService> _logger)
        {
            repo = _repo;
            logger = _logger;
        }
        public async Task<bool> AddBlockAsync(AddBlockedCountryDto blockDto)
        {
            var countryCode = blockDto.CountryCode?.Trim().ToUpperInvariant() ?? "";
           
            CheckCountryCodeWithLogging(countryCode);
            var existing = await repo.GetBlockAsync(countryCode);
            if (existing != null)
            {
                logger.LogInformation("Country code {CountryCode} is already blocked.", countryCode);
                return false;
            }
            var countryBlock = new Models.CountryBlock
            {
                CountryCode = countryCode,
                CountryName = "",
                Blocked=true,
                BlockedUntilUtc = null // Set to null for permanent blockDto
            };
            var created = await repo.AddBlockAsync(countryBlock);
            if (!created)
            {

                logger.LogError("Failed to blockDto country code {CountryCode}.", countryCode);
                return false;
            }
            logger.LogInformation("Added permanent blockDto for {Country}", countryCode);
            return true;


        }

        public async Task<bool> AddTemporalBlockAsync(TemporalBlockDto blockDto)
        {
            var countryCode = blockDto.CountryCode?.Trim().ToUpperInvariant() ?? "";

            CheckCountryCodeWithLogging(countryCode);
           await IsBlockedCountryWithLogging(countryCode);
            var countryBlock = new Models.CountryBlock
            {
                CountryCode = countryCode,
                CountryName = "",
                BlockedUntilUtc = DateTime.UtcNow.AddMinutes(blockDto.DurationMinutes)
            };
            var created = await repo.AddBlockAsync(countryBlock);
            if (!created)
            {
                logger.LogError("Failed to add temporal blockDto for country code {CountryCode}.", countryCode);
                return false;
            }
            logger.LogInformation("Added temporal blockDto for {Country} until {BlockedUntilUtc}", countryCode, countryBlock.BlockedUntilUtc);
            return true;

        }

        public async Task<PagedResult<CountryBlock>> GetAllAsync(int page, int pageSize, string? search)
        {
            logger.LogInformation("Fetching blocked countries (Page: {Page}, PageSize: {PageSize}, Search: {Search})", page, pageSize, search);

            var blockedCountries = await repo.GetAllAsync();

            logger.LogDebug("Retrieved {Count} blocked countries from repository", blockedCountries.Count());

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToUpperInvariant();

                logger.LogInformation("Applying search filter with term: {Term}", term);

                blockedCountries = blockedCountries
                    .Where(c => c.CountryCode.ToUpperInvariant().Contains(term)
                             || (!string.IsNullOrEmpty(c.CountryName) && c.CountryName.ToUpperInvariant().Contains(term)));


                logger.LogDebug("After search filter, {FilteredCount} countries remain", blockedCountries.Count());
            }

            var totalCount = blockedCountries.Count();
            var items = blockedCountries
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            logger.LogInformation("Returning paged result: {ReturnedCount}/{TotalCount} countries (Page: {Page})",
                items.Count, totalCount, page);

            return new PagedResult<CountryBlock>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }


        public async Task<bool> RemoveBlockAsync(RemoveBlockedCountryDto removeCountryDto)
        {
            var countryCode = removeCountryDto.CountryCode?.Trim().ToUpperInvariant() ?? "";
            if (string.IsNullOrEmpty(countryCode) || countryCode.Length != 2)
            {
                logger.LogWarning("Invalid country code provided: {CountryCode}", countryCode);
                return false;
            }
            await IsBlockedCountryWithLogging(countryCode);
            var removed = await repo.RemoveBlockedAsync(countryCode);
            if (!removed)
            {
                logger.LogError("Failed to remove blockDto for country code {CountryCode}.", countryCode);
                return false;
            }
            logger.LogInformation("Removed blockDto for {Country}", countryCode);
            return true;


        }
        private bool IsValidCountryCode(string code)
        {
            try
            {
                var region = new System.Globalization.RegionInfo(code);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool CheckCountryCodeWithLogging(string countryCode)
        {
            if (!IsValidCountryCode(countryCode))
            {
                logger.LogWarning("Invalid country code provided: {CountryCode}", countryCode);
                return false;
            }
            return true;
        }
        private async Task<bool> IsBlockedCountryWithLogging(string countryCode)
        {
            var existing = await repo.GetBlockAsync(countryCode);
            if (existing != null)
            {
                logger.LogInformation("Country code {CountryCode} is already blocked.", countryCode);
                return false;
            }
            return true;
        }

    }

}
