using Models;

namespace Domian.ReposContract
{
    public interface IBlockedAttemptLogRepo
    {

        Task AddLogAsync(BlockLog log);
        Task<IEnumerable<BlockLog>> GetAllAsync();

    }
}
