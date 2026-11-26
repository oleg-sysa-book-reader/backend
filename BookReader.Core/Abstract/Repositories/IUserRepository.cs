using BookReader.Core.Entities;

namespace BookReader.Core.Abstract.Repositories
{
    public interface IUserRepository : IRepository
    {
        Task<User?> GetAsync(string email, CancellationToken token);
        Task<User?> GetByExternalIdAsync(string id, CancellationToken token);
        Task AddAsync(User user);
    }
}
