using BookReader.Core.Entities;
using BookReader.Core.Enums;

namespace BookReader.Core.Abstract.Repositories
{
    public interface IOutboxMessageRepository : IRepository
    {
        Task AddAsync(OutboxMessage message, CancellationToken token, bool saveChanges = true);
        Task<IEnumerable<OutboxMessage>> GetMessagesForProcessingAsync(CancellationToken token, OutboxMessageType? type = null);
    }
}
