using Application.DTOs;
using Models;

namespace Application.Services
{
    public interface ICountryBlockService
    {
        Task<bool> AddBlockAsync(AddBlockedCountryDto block);
        Task<bool> RemoveBlockAsync(RemoveBlockedCountryDto removeDto);
        Task<PagedResult<CountryBlock>> GetAllAsync(int page,int pageSize,string? search);
        Task<bool> AddTemporalBlockAsync(TemporalBlockDto blockDto);
    }
}
