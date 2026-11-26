using BookReader.Core.DTOs.Models;
using BookReader.Core.Enums;

namespace BookReader.Core.Abstract.Services
{
    public interface IParser
    {
        BookExtension Extension { get; }
        //Task<Dictionary<int, string>> ParseFile(string path);
        Task<CompositeNode> ParseFile(string path);
    }
}
