using BookReader.Core.Abstract.Services;
using BookReader.Core.DTOs.Models;
using ClosedXML.Excel;

namespace BookReader.Infrastructure.Services;

public sealed class ExcelService : IExcelService
{
    public Stream ExportLearningWords(IReadOnlyCollection<LearningWordModel> words)
    {
        ArgumentNullException.ThrowIfNull(words);

        var stream = new MemoryStream();
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Learning words");

            worksheet.Cell(1, 1).Value = "English word";
            worksheet.Cell(1, 2).Value = "Translation";
            worksheet.Cell(1, 3).Value = "English example sentence";
            worksheet.Cell(1, 4).Value = "Example sentence translation";
            worksheet.Range(1, 1, 1, 4).Style.Font.Bold = true;

            var row = 2;
            foreach (var word in words)
            {
                worksheet.Cell(row, 1).Value = word.SourceValue;
                worksheet.Cell(row, 2).Value = word.Translation;
                worksheet.Cell(row, 3).Value = string.Empty;
                worksheet.Cell(row, 4).Value = string.Empty;
                row++;
            }

            worksheet.Columns(1, 4).AdjustToContents();
            workbook.SaveAs(stream);
        }

        stream.Position = 0;
        return stream;
    }
}
