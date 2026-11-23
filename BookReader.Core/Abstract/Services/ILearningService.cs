using BookReader.Core.DTOs.Models;

namespace BookReader.Core.Abstract.Services
{
    public interface ILearningService
    {
        Task<ServiceResult<bool>> LearnWord(int userId, string word, CancellationToken token);
        Task<ServiceResult<LearningWordsResult>> GetUserVocabulary(int userId, int pageSize, int pageNumber, CancellationToken token);
    }
}
