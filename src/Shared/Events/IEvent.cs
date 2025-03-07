namespace Shared.Events
{
    public interface IEvent
    {
        public static abstract string Topic { get; }
    }
}
