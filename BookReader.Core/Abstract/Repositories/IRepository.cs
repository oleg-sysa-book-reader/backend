namespace BookReader.Core.Abstract.Repositories
{
    public interface IRepository
    {
        Task SaveChangesAsync(CancellationToken token);
    }
}
