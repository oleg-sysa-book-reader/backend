using BookReader.Core.Abstract.Repositories;
using BookReader.Core.Entities;
using BookReader.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookReader.Infrastructure.Repositories
{
    public class TranslationRepository : BaseRepository, ITranslationRepository
    {
        public TranslationRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<int> AddTranslationAsync(Translation translation, CancellationToken token)
        {
            _context.Translations.Add(translation);
            var res = await _context.SaveChangesAsync();
            return translation.Id;
        }

        public async Task AddTranslationBatchAsync(List<Translation> toAddList, CancellationToken token)
        {
            foreach (var translationsChunk in toAddList.Chunk(CHUNK_SIZE))
            {
               await _context.Translations.AddRangeAsync(translationsChunk);
               await _context.SaveChangesAsync();
            }
        }

        public async Task<ICollection<Translation>> GetAllAsNoTrackingAsync(CancellationToken token) =>
            await _context.Translations.AsNoTracking().ToListAsync(token);

        public Task<List<Translation>> GetAllTranslationsByValuesAsync(List<string> values, CancellationToken token)
        {
           return _context.Translations
                .Where(t => values.Contains(t.SourceWord))
                .ToListAsync(token);
        }

        public async Task<Translation?> GetAsync(string sourceLang, string targetLang, string input, CancellationToken token)
        {
            return await _context.Translations.FirstOrDefaultAsync(t => 
                t.SourceLang == sourceLang &&
                t.TargetLang == targetLang &&
                t.SourceWord == input, token);
        }
    }
}
