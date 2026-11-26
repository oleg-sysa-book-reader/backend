using BookReader.API.Models.Requests;
using BookReader.Core.Abstract.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookReader.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearningController : BaseAPIController
    {
        private readonly ILearningService _learningService;
        public LearningController(ILearningService learningService)
        {
            _learningService = learningService;
        }

        [HttpPost]
        public async Task<IActionResult> AddToLearning([FromBody] string value, CancellationToken token)
        {
            var res = await _learningService.LearnWord(UserId, value, token);
            var code = res.IsSuccess ?
                StatusCodes.Status200OK :
                StatusCodes.Status400BadRequest;
            return GenerateResponse(code, res);
        }

        [HttpDelete("{learningWordId:int}")]
        public async Task<IActionResult> DeleteLearningWord(int learningWordId, CancellationToken token)
        {
            var res = await _learningService.DeleteUserLearningWordAsync(UserId, learningWordId, token);
            var code = res.IsSuccess ?
                StatusCodes.Status200OK :
                StatusCodes.Status404NotFound;
            return GenerateResponse(code, res);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserVocabulary([FromQuery]BasePaginationRequest request, CancellationToken token)
        {
            var res = await _learningService.GetUserVocabulary(UserId, request.pageSize, request.pageNumber, token);
            var code = res.IsSuccess ?
                StatusCodes.Status200OK :
                StatusCodes.Status400BadRequest;
            return GenerateResponse(code, res);
        }

        [HttpPost("import")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ImportWords([FromForm] IFormFile file, CancellationToken token)
        {
            if (file == null || file.Length == 0)
                return BadRequest("An Excel file is required.");

            if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Only .xlsx files are supported.");

            await using var stream = file.OpenReadStream();
            var res = await _learningService.ImportUserLearningWordsAsync(UserId, stream, token);
            var code = res.IsSuccess ?
                StatusCodes.Status200OK :
                StatusCodes.Status400BadRequest;
            return GenerateResponse(code, res);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportWords(CancellationToken token)
        {
            var res = await _learningService.ExportUserLearningWordsAsync(UserId, token);
            return File(
                res.Data!.result,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "learning-words.xlsx");
        }
    }
}
