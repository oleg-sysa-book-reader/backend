using System.Text.Json.Serialization;
using BookReader.Core.DTOs.Models;
using BookReader.Core.Enums;

namespace BookReader.Core.Abstract.Business
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(TextNode), "text")]
    [JsonDerivedType(typeof(CompositeNode), "composite")]
    public interface ITextElement
    {
        TextNodeType NodeType { get; }
        int CharsCount { get; }
        Dictionary<string, string> Attributes { get; set; }
        string GetText();

    }
}
