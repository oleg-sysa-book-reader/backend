using BookReader.Core.Enums;

namespace BookReader.Core.Abstract.Events
{
    public interface IBusinessEvent
    {
        EventTypes Type { get; }
    }
}
