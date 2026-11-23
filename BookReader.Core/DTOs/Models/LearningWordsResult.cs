namespace BookReader.Core.DTOs.Models
{
    public record LearningWordsResult(int totalPages, int totalCount, List<LearningWordModel> data);
}
