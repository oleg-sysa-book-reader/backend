using BookReader.Core.Abstract.Repositories;
using BookReader.Core.DTOs.Models;
using BookReader.Core.Entities;
using BookReader.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookReader.Infrastructure.Repositories
{
    public class LearningRepository : BaseRepository, ILearningRepository
    {
        public LearningRepository(AppDbContext context) : base(context)
        {
        }
        public async Task AddWordAsync(LearningWord learningWord, CancellationToken token)
        {
            _context.LearningWords.Add(learningWord);
            await SaveChangesAsync(token);
        }

        public async Task AddWordsBatchAsync(IReadOnlyCollection<LearningWord> learningWords, CancellationToken token, bool enforceSaving = true)
        {
            if (learningWords.Count == 0)
                return;

            _context.LearningWords.AddRange(learningWords);
            if (enforceSaving)
            {
                await _context.SaveChangesAsync(token);
            }
        }

        public async Task<bool> DeleteWordAsync(int userId, int learningWordId, CancellationToken token)
        {
            var learningWord = await _context.LearningWords
                .FirstOrDefaultAsync(word => word.Id == learningWordId && word.UserId == userId, token);
            if (learningWord == null)
                return false;

            _context.LearningWords.Remove(learningWord);
            await _context.SaveChangesAsync(token);
            return true;
        }

        public async Task<List<LearningWord>> GetLearningWords(int userId, CancellationToken token, int? translationId = null) =>
            await _context.LearningWords
                .Where(lw => lw.UserId == userId && (translationId == null || translationId == lw.TranslationId))
                .ToListAsync(token);

        public Task<List<LearningWordModel>> GetLearningWordsWithValues(int userId, CancellationToken token, int? translationId = null)
        {
            return _context.LearningWords
                .Include(lw => lw.Translation)
                .Where(lw => lw.UserId == userId && (translationId == null || translationId == lw.TranslationId))
                .Select(lw => new LearningWordModel()
                {
                    SourceValue = lw.Translation.SourceWord,
                    Id = lw.Id,
                    TranslationId = lw.TranslationId,
                    Translation = lw.Translation.TranslatedWord,
                    UserId = lw.UserId,
                }).ToListAsync();
        }

    }
}
