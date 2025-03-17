namespace ServiceDefaults.Events
{
    public record ProductAddedEvent(string Id, string Description, decimal Price) : IEvent
    {
        public static string Topic => "productAdded";
    }
}
