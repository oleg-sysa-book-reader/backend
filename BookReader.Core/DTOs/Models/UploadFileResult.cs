using BookReader.Core.Enums;

namespace BookReader.Core.DTOs.Models
{
    public sealed record UploadFileResult(BookStatus Status, string Path);
}
