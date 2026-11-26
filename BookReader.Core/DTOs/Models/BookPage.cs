using BookReader.Core.Abstract.Business;

namespace BookReader.Core.DTOs.Models
{
    public class BookPage
    {
        public int Number { get; set; }
        public required List<ITextElement> Paragraphs { get; set; }
    }
}
