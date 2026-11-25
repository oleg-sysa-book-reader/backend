using BookReader.Core.DTOs.Models;
using BookReader.Core.Entities;

namespace BookReader.Core.Abstract.Services
{
    public interface ITranslationService
    {
        Task<ServiceResult<string>> TranslateAsync(string input, CancellationToken token);
        Task<ServiceResult<List<Translation>>> TranslateBatchAsync(List<string> words, CancellationToken token);
        Task<ServiceResult<string>> TranslateSentenceAsync(int sentenceId, string value, CancellationToken token);
    }
}
