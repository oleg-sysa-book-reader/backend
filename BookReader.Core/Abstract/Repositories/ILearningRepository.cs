using BookReader.Core.DTOs.Models;
using BookReader.Core.Entities;

namespace BookReader.Core.Abstract.Repositories
{
    public interface ILearningRepository : IRepository
    {
        Task AddWordAsync(LearningWord word, CancellationToken token);
        Task AddWordsBatchAsync(IReadOnlyCollection<LearningWord> learningWords, CancellationToken token, bool enforceSaving = true);
        Task<bool> DeleteWordAsync(int userId, int learningWordId, CancellationToken token);
        Task<List<LearningWord>> GetLearningWords(int userId, CancellationToken token, int? translationId = null);
        Task<List<LearningWordModel>> GetLearningWordsWithValues(int userId, CancellationToken token, int? translationId = null);
    }
}
