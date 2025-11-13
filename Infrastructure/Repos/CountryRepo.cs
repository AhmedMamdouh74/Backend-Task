using Domain.Repos;
using Models;
using System.Collections.Concurrent;

namespace Infrastructure.Repos
{
    public class CountryRepo : ICountryRepo
    {
        private readonly ConcurrentDictionary<string, CountryBlock> blocks = new(StringComparer.OrdinalIgnoreCase);
        public CountryRepo(ConcurrentDictionary<string, CountryBlock> _blocks)
        {
            blocks = _blocks;
        }
        public Task<bool> AddBlockAsync(CountryBlock block)
        {
            var added = blocks.TryAdd(block.CountryCode.ToUpperInvariant(), block);
            return Task.FromResult(added);
        }

        public Task<bool> RemoveBlockedAsync(string countryCode)
        {
            var removed = blocks.TryRemove(countryCode.ToUpperInvariant(), out _);
            return Task.FromResult(removed);
        }

        public Task<CountryBlock?> GetBlockAsync(string countryCode)
        {
            countryCode = countryCode.ToUpperInvariant();
            blocks.TryGetValue(countryCode, out var block);
            return Task.FromResult(block);
        }

        public Task<IEnumerable<CountryBlock>> GetAllAsync()
        {
            return Task.FromResult(blocks.Values.AsEnumerable());
        }

        public Task<bool> AddTemporalBlockAsync(CountryBlock countryBlock)
        {
            var added = blocks.TryAdd(countryBlock.CountryCode.ToUpperInvariant(), countryBlock);
            return Task.FromResult(added);
        }
    }
}
