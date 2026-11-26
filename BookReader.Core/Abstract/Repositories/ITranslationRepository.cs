using BookReader.Core.Entities;

namespace BookReader.Core.Abstract.Repositories
{
    public interface ITranslationRepository : IRepository
    {
        public Task<int> AddTranslationAsync(Translation translation, CancellationToken token);
        public Task AddTranslationBatchAsync(List<Translation> toAddList, CancellationToken token, bool enforceSaving = true);
        public Task<Translation?> GetAsync(string sourceLang, string targetLang, string input, CancellationToken token);
        public Task<ICollection<Translation>> GetAllAsNoTrackingAsync(CancellationToken token);
        public Task<List<Translation>> GetAllTranslationsByValuesAsync(List<string> values, CancellationToken token);
    }
}
