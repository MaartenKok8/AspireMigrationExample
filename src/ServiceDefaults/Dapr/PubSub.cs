using Dapr.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceDefaults.Events;

namespace ServiceDefaults.Dapr
{
    /// <summary>
    /// Dapr pub sub abstraction, allowing for unit test mocking as well as abstracting away the pub sub name used.
    /// </summary>
    public interface IPubSub
    {
        Task PublishEventAsync<TEvent>(TEvent @event) where TEvent : IEvent;
    }

    class PubSub(DaprClient daprClient, IOptionsMonitor<DaprOptions> options, ILogger<PubSub> logger) : IPubSub
    {
        public async Task PublishEventAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var pubSubName = options.CurrentValue.PubSub;
            await daprClient.PublishEventAsync(pubSubName, TEvent.Topic, @event);
            logger.LogInformation("Published to topic '{Topic}' using pubsub '{PubSub}'", TEvent.Topic, pubSubName);
        }
    }
}
