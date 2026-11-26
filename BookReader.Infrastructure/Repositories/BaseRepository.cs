using BookReader.Infrastructure.Persistence;
namespace BookReader.Infrastructure.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly AppDbContext _context;
        protected const int CHUNK_SIZE = 500;
        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task SaveChangesAsync(CancellationToken token)
        {
            return _context.SaveChangesAsync(token);
        }
    }
}
