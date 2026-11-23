using BookReader.Core.Abstract.Repositories;
using BookReader.Core.Abstract.Services;
using BookReader.Core.DTOs.Models;
using BookReader.Core.Entities;
using BookReader.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
namespace BookReader.Core.Business
{
    public class LearningService : BaseService<LearningService>, ILearningService
    {
        private readonly ILearningRepository _learningRepository;
        private readonly ITranslationRepository _translationRepository;
        private readonly ICacheService _cacheService;
        public LearningService(ICacheService cacheService,
            ITranslationRepository translationRepository,
            ILearningRepository learningRepository,
            IConfiguration config,
            ILogger<LearningService> logger,
            IHttpContextAccessor httpContextAccessor) : base(config, logger, httpContextAccessor)
        {
            _cacheService = cacheService;
            _translationRepository = translationRepository;
            _learningRepository = learningRepository;
        }

        public async Task<ServiceResult<LearningWordsResult>> GetUserVocabulary(int userId, int pageSize, int pageNumber, CancellationToken token)
        {
            var cachekey = $"user-vocabulary-{userId}";
            var data = await _cacheService.GetAsync<List<LearningWordModel>>(cachekey);
            if (data.IsNullOrEmpty())
            {
                data = await _learningRepository.GetLearningWordsWithValues(userId, token);
                await _cacheService.SetAsync(cachekey, data);
            }
            
            var paginated = data?.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList() ?? new List<LearningWordModel>();
            var totalPages = (int)Math.Ceiling((double)(data?.Count ?? 0) / pageSize);
            var totalCount = data?.Count ?? 0;
            var response = new LearningWordsResult(totalPages, totalCount, paginated);
            return new ServiceResult<LearningWordsResult>(response, null);
        }

        public async Task<ServiceResult<bool>> LearnWord(int userId, string word, CancellationToken token)
        {
            var sourceLang = _config["Translation:SourceLang"]!;
            var targetLang = _config["Translation:TargetLang"]!;
            var translationEntity = await _translationRepository.GetAsync(sourceLang, targetLang, word, token);
            if (translationEntity == null) 
                return new ServiceResult<bool>(false, $"Translation for word '{word}' not found.");

            var savedWords = await _learningRepository.GetLearningWords(userId, token, translationEntity.Id);
            if(!savedWords.IsNullOrEmpty())
                return new ServiceResult<bool>(false, $"This word already in learning from this user.");

            var learnWord = new LearningWord
            {
                UserId = userId,
                TranslationId = translationEntity.Id,
            };
            await _learningRepository.AddWordAsync(learnWord, token);
            return new ServiceResult<bool>(true, null);
        }
    }
}
