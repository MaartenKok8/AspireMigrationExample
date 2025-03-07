using Dapr.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Shared.Dapr
{
    /// <summary>
    /// Dapr state store abstraction, allowing for unit test mocking as well as abstracting away the state store name used.
    /// </summary>
    public interface IStateStore
    {
        Task SaveStateAsync<T>(string key, T value, IReadOnlyDictionary<string, string>? metadata = null);
        Task<T?> GetStateAsync<T>(string key);
    }

    class StateStore(DaprClient daprClient, IOptionsMonitor<DaprOptions> options, ILogger<StateStore> logger) : IStateStore
    {
        public async Task SaveStateAsync<T>(string key, T value, IReadOnlyDictionary<string, string>? metadata = null)
        {
            var stateStore = options.CurrentValue.StateStore;
            await daprClient.SaveStateAsync(options.CurrentValue.StateStore, key, value, metadata: metadata ?? new Dictionary<string, string>());
            logger.LogDebug("Saved to state store '{StateStore}', to key '{Key}'", stateStore, key);
        }

        public async Task<T?> GetStateAsync<T>(string key)
        {
            var stateStore = options.CurrentValue.StateStore;
            var result = await daprClient.GetStateAsync<T>(stateStore, key);
            logger.LogDebug("Retrieved state from '{StateStore}', key '{Key}'", stateStore, key);
            return result;
        }
    }
}
