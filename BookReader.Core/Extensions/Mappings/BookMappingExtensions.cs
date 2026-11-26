using BookReader.Core.DTOs.Models;
using BookReader.Core.Entities;

namespace BookReader.Core.Extensions.Mappings
{
    public static class BookMappingExtensions
    {
        public static BookModel ToDto(this Book book)
        {
            return new BookModel(
                book.Id,
                book.OriginalFileName,
                book.FileSize,
                (int)book.Status,
                book.CreatedAtUtc);
        }

        public static IEnumerable<BookModel> ToDto(
            this IEnumerable<Book> books)
        {
            return books.Select(x => x.ToDto());
        }
    }
}
