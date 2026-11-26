namespace BookReader.Core.DTOs.Models
{
    public record BookDeletedPayload(int UserId, int BookId, string OriginalFileName);
}
