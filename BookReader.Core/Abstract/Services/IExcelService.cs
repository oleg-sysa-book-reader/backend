using BookReader.Core.DTOs.Models;

namespace BookReader.Core.Abstract.Services;

public interface IExcelService
{
    Stream ExportLearningWords(IReadOnlyCollection<LearningWordModel> words);
    IReadOnlyCollection<string> ReadLearningWords(Stream stream);
}
