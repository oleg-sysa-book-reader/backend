using BookReader.Core.Abstract.Events;
using BookReader.Core.Enums;

namespace BookReader.Core.Events
{
    public sealed record BookNotificationEvent(int UserId, int BookId, BookStatus Status, string? Message = null) : IBusinessEvent
    {
        public EventTypes Type => EventTypes.BookNotificationEvent;
    }
}
