using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models;
using System.Collections.Concurrent;
namespace Infrastructure.Services;

public class TemporalBlockCleanupService : BackgroundService
{
    private readonly ILogger<TemporalBlockCleanupService> logger;
    private readonly ConcurrentDictionary<string, CountryBlock> blockedCountries;

    public TemporalBlockCleanupService(
        ILogger<TemporalBlockCleanupService> _logger,
        ConcurrentDictionary<string, CountryBlock> _blockedCountries)
    {
        logger = _logger;
        blockedCountries = _blockedCountries;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;

            var expired = blockedCountries
                .Where(kvp => kvp.Value.BlockedUntilUtc.HasValue &&
                              kvp.Value.BlockedUntilUtc <= now)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var code in expired)
            {
                if (blockedCountries.TryRemove(code, out var block))
                {
                    logger.LogInformation("Removed expired temporal block for {Code}", code);
                }
            }

            // Run every 5 minutes
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
