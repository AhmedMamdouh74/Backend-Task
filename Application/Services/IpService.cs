using Application.DTOs;
using Domain.Repos;
using Domian.ReposContract;
using Microsoft.Extensions.Logging;
using Models;

namespace Application.Services
{
    public class IpService : IIpService
    {
        private readonly IGeoLocationService geoService;
        private readonly ICountryRepo countryRepo;
        private readonly IBlockedAttemptLogRepo logRepo;
        private readonly ILogger<IpService> logger;

        public IpService(
            IGeoLocationService _geoService,
            ICountryRepo _countryRepo,
            IBlockedAttemptLogRepo _logRepo,
            ILogger<IpService> _logger)
        {
            geoService = _geoService;
            countryRepo = _countryRepo;
            logRepo = _logRepo;
            logger = _logger;
        }

        public async Task<IpCheckResponseDto> CheckBlockAsync(string ipAddress, string userAgent)
        {
            logger.LogInformation("Checking block status for IP: {IpAddress}, UserAgent: {UserAgent}", ipAddress, userAgent);

            var result = await geoService.GetCountryByIpAsync(ipAddress);
            logger.LogDebug("Geo lookup result: CountryCode={CountryCode}, CountryName={CountryName}", result.CountryCode, result.CountryName);

            var country = await countryRepo.GetBlockAsync(result.CountryCode);
            bool isBlocked = country != null && country.Blocked;
            var logs = await logRepo.GetAllAsync();
            var alreadyLogged = logs.Any(country => country.Ip == ipAddress);
            // to avoid dublicates
            if(!alreadyLogged)
            await logRepo.AddLogAsync(new BlockLog
            {
                CountryCode = result.CountryCode,
                Blocked = isBlocked,
                Ip = ipAddress,
                UserAgent = userAgent
            });

            if (country == null)
            {
                logger.LogInformation("Country {CountryCode} is not blocked.", result.CountryCode);
                return new IpCheckResponseDto(ipAddress, result.CountryCode, false);
            }






            logger.LogInformation("Country {CountryCode} block expired. Returning temporal block status.", country.CountryCode);
            return new IpCheckResponseDto(ipAddress, country.CountryCode, country.Blocked);
        }

        public async Task<PagedResult<BlockLog>> GetLogsAsync(int page, int pageSize)
        {
            logger.LogInformation("Fetching logs with pagination: Page={Page}, PageSize={PageSize}", page, pageSize);

            var logs = await logRepo.GetAllAsync();

            if (logs == null || !logs.Any())
            {
                logger.LogWarning("No logs found in repository.");
                return new PagedResult<BlockLog>
                {
                    Items = new List<BlockLog>(),
                    TotalCount = 0,
                    Page = page,
                    PageSize = pageSize
                };
            }

            var total = logs.Count();
            logger.LogDebug("Total logs retrieved: {TotalCount}", total);

            var items = logs
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(block => new BlockLog
                {
                    Blocked = block.Blocked,
                    CountryCode = block.CountryCode,
                    Ip = block.Ip,
                    TimestampUtc = block.TimestampUtc,
                    UserAgent = block.UserAgent
                })
                .ToList();

            logger.LogInformation("Returning {ItemCount} logs for Page={Page}, PageSize={PageSize}", items.Count, page, pageSize);

            return new PagedResult<BlockLog>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }


        public async Task<IpLookupResponseDto> LookupAsync(string ipAddress)
        {
            logger.LogInformation("Looking up IP: {IpAddress}", ipAddress);

            var geoInfo = await geoService.GetCountryByIpAsync(ipAddress);

            logger.LogDebug("Geo lookup for {IpAddress}: CountryCode={CountryCode}, CountryName={CountryName}, ISP={ISP}",
                ipAddress, geoInfo.CountryCode, geoInfo.CountryName, geoInfo.ISP);

            return new IpLookupResponseDto
            (
                ipAddress,
                geoInfo.CountryCode,
                geoInfo.CountryName,
                geoInfo.ISP
            );
        }
    }
}
