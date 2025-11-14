using Domian.ReposContract;
using Models;
using System.Collections.Concurrent;

namespace Infrastructure.Repos
{
    internal class BlockedAttemptLogRepo : IBlockedAttemptLogRepo
    {
        private readonly ConcurrentBag<BlockLog> logs = new ConcurrentBag<BlockLog>();
        public Task AddLogAsync(BlockLog log)
        {

            logs.Add(log);
            return Task.CompletedTask;

        }

        public Task<IEnumerable<BlockLog>> GetAllAsync()
        {
            return Task.FromResult(logs.AsEnumerable());
        }
    }
}
