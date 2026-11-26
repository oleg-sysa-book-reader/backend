using BookReader.Core.Abstract.Repositories;
using BookReader.Infrastructure.Persistence;

namespace BookReader.Infrastructure.Repositories
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }
    }
}
