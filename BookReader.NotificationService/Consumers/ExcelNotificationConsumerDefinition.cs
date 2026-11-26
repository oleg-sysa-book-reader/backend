using MassTransit;

namespace BookReader.NotificationService.Consumers
{
    public class ExcelNotificationConsumerDefinition : ConsumerDefinition<ExcelNotificationConsumer>
    {
        public ExcelNotificationConsumerDefinition()
        {
            EndpointName = "excel-notifications";
        }
    }
}
