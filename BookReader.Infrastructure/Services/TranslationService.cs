using BookReader.Core.Abstract.Repositories;
using BookReader.Core.Abstract.Services;
using BookReader.Core.Business;
using BookReader.Core.DTOs.Models;
using BookReader.Core.Entities;
using BookReader.Core.Extensions;
using DeepL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BookReader.Infrastructure.Services
{
    public class TranslationService : BaseService<TranslationService>, ITranslationService
    {
        private readonly ICacheService _cacheService;
        private readonly ITranslationRepository _translationRespository;
        public TranslationService(ICacheService cacheService,
            ITranslationRepository translationRespository,
            IConfiguration config,
            ILogger<TranslationService> logger,
            IHttpContextAccessor httpContextAccessor) : base(config, logger, httpContextAccessor)
        {
            _cacheService = cacheService;
            _translationRespository = translationRespository;
        }
        public async Task<ServiceResult<string>> TranslateAsync(string input, CancellationToken token)
        {
            try
            {
                var sourceLang = _config["Translation:SourceLang"]!;
                var targetLang = _config["Translation:TargetLang"]!;
                var translationKey = input.BuildCacheTranslationKey(sourceLang, targetLang);
                var cachedTranslation = await _cacheService.GetAsync<string>(translationKey);
                if (!string.IsNullOrEmpty(cachedTranslation))
                    return new ServiceResult<string>(cachedTranslation, string.Empty);

                var translationResult = string.Empty;
                var translationEntity = await _translationRespository.GetAsync(sourceLang, targetLang, input, token);
                if (translationEntity == null)
                {
                    translationResult = await ExecuteTraslation(input, token);
                    translationEntity = new Translation()
                    {
                        SourceLang = sourceLang,
                        TargetLang = targetLang,
                        SourceWord = input,
                        TranslatedWord = translationResult
                    };
                    await _translationRespository.AddTranslationAsync(translationEntity, token);
                }
                else
                    translationResult = translationEntity.TranslatedWord;


                await _cacheService.SetAsync<string>(translationKey, translationResult);
                return new ServiceResult<string>(translationResult, string.Empty);
            }
            catch (Exception e)
            {
                _logger.LogError("Cannot translate input string. Error: {message}", e.Message);
                return new ServiceResult<string>(string.Empty, e.Message);
            }
        }

        public async Task<ServiceResult<List<Translation>>> TranslateBatchAsync(List<string> words, CancellationToken token)
        {
            try
            {
                var sourceLang = _config["Translation:SourceLang"]!;
                var targetLang = _config["Translation:TargetLang"]!;
                var translationResult = await ExecuteBatchTraslation(words, token);
                var newTranslations = new List<Translation>();
                foreach (var toAdd in translationResult)
                {
                    var translationEntity = new Translation()
                    {
                        SourceLang = sourceLang,
                        TargetLang = targetLang,
                        SourceWord = toAdd.Key,
                        TranslatedWord = toAdd.Value
                    };
                    newTranslations.Add(translationEntity);
                    var translationKey = translationEntity.SourceWord.BuildCacheTranslationKey(sourceLang, targetLang);
                    await _cacheService.SetAsync<string>(translationKey, toAdd.Value);
                }
                if (!newTranslations.IsNullOrEmpty())
                {
                    await _translationRespository.AddTranslationBatchAsync(newTranslations, token);
                }

                return new ServiceResult<List<Translation>>(newTranslations, string.Empty);
            }
            catch (Exception e)
            {
                _logger.LogError("Cannot translate input string. Error: {message}", e.Message);
                return new ServiceResult<List<Translation>>(new List<Translation>(), e.Message);
            }
        }

        public async Task<ServiceResult<string>> TranslateSentenceAsync(int sentenceId, string value, CancellationToken token)
        {
            try
            {
                var result = await ExecuteTraslation(value, token);
                return new ServiceResult<string>(result, string.Empty);
            }
            catch (Exception e)
            {
                _logger.LogError("Cannot translate input string. Error: {message}", e.Message);
                return new ServiceResult<string>(string.Empty, e.Message);
            }
        }

        private async Task<string> ExecuteTraslation(string input, CancellationToken token)
        {
            var apikey = _config["ApiKeys:DeplKey"]!;
            var client = new DeepLClient(apikey);

            var translatedText = await client.TranslateTextAsync(
                input,
                null,
                LanguageCode.Ukrainian, cancellationToken: token);
            return translatedText.Text;
        }

        private async Task<Dictionary<string, string>> ExecuteBatchTraslation(List<string> inputs, CancellationToken token)
        {
            var apikey = _config["ApiKeys:DeplKey"]!;
            var client = new DeepLClient(apikey);

            var translatedList = await client.TranslateTextAsync(
                inputs,
                null,
                LanguageCode.Ukrainian, cancellationToken: token);

            return inputs.Select((input, index) => new
            {
                Original = input,
                Translation = translatedList[index].Text
            }).ToDictionary(x => x.Original, x => x.Translation);
        }
    }
}
