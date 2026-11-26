namespace BookReader.Core.Abstract.Events
{
    public interface IEventHandler<TEvent> where TEvent : IBusinessEvent
    {
        Task HandleAsync(TEvent e, CancellationToken token);
    }
}
