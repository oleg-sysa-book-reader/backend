namespace BookReader.Core.Extensions
{
    public static class CacheExtensions
    {
        public static string BuildCacheTranslationKey(this string input, string sourceLang, string targetLang) =>
            $"translation:{sourceLang}:{targetLang}:{input.ToLower()}";
        public static string BuildCacheBookKey(int userId, int bookId) =>
            $"parsedbooks:{userId}:{bookId}";
        public static string BuildCacheLearningWordsKey(int userId) =>
          $"user-vocabulary-{userId}";

    }
}
