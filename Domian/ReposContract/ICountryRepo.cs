using Models;

namespace Domain.Repos
{
    public interface ICountryRepo
    {
        Task<bool> AddBlockAsync(CountryBlock block);
        Task<CountryBlock?> GetBlockAsync(string countryCode);
        Task<bool> RemoveBlockedAsync(string countryCode);
        Task<IEnumerable<CountryBlock>> GetAllAsync();
        Task<bool> AddTemporalBlockAsync(CountryBlock countryBlock);
    }
}
