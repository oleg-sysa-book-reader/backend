using BookReader.Core.Events;
using BookReader.NotificationService.Abstract;
using MassTransit;

namespace BookReader.NotificationService.Consumers
{
    public class ExcelNotificationConsumer : IConsumer<ExcelImportNotificationEvent>
    {
        private readonly INotificationManager _notificationManager;
        public ExcelNotificationConsumer(INotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
        }
        public async Task Consume(ConsumeContext<ExcelImportNotificationEvent> context)
        {
            using (Serilog.Context.LogContext.PushProperty("CorrelationId", context.CorrelationId))
            {
                await _notificationManager.SendAsync(context.Message.UserId, context.Message);
            }
        }
    }
}
