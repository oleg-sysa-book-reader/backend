using BookReader.Core.DTOs.Models;
namespace BookReader.Core.Abstract.Services
{
    public interface IDocumentNodeService
    {
        Task<ServiceResult<BookViewResult>> GetPageContentAsync(int userId, int bookId,
            int pageNumber,
            bool useBookmark,
            CancellationToken token);
    }
}
