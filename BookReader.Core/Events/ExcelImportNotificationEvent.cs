using BookReader.Core.Abstract.Events;
using BookReader.Core.Enums;

namespace BookReader.Core.Events
{
    public record ExcelImportNotificationEvent(int UserId, ExcelImportStatus status, string? error = null) : IBusinessEvent
    {
        public EventTypes Type => EventTypes.ExcelImportNotificationEvent;
    }
}
