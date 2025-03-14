using System.Net.Http.Json;
using System.Text.Json;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests
{
    [Collection(nameof(DistributedApplicationCollection))]
    public class IntegrationTest(DistributedApplicationFixture distributedAppFixture)
    {
        protected async Task<HttpClient> GetGatewayClientAsync()
        {
            var distributedApp = await distributedAppFixture.GetDistributedApplicationAsync();
            
            var resources = distributedApp.Services.GetRequiredService<ResourceNotificationService>();
            await resources.WaitForResourceHealthyAsync("apigateway-dapr-cli") // Note we are waiting for the dapr sidecar to become active
                .WaitAsync(TimeSpan.FromSeconds(120));
            
            return distributedApp.CreateHttpClient("apigateway");
        }

        protected async Task<T> PollGatewayUntilAsync<T>(Func<T, bool> check, string url) => await PollUntilAsync(await GetGatewayClientAsync(), check, url);

        private async Task<T> PollUntilAsync<T>(HttpClient httpClient, Func<T, bool> check, string url)
        {
            T? response = default;
            HttpRequestException? lastException = null;

            for (int i = 0; i < 30; i++)
            {
                lastException = null;
                response = default;

                try
                {
                    response = await httpClient.GetFromJsonAsync<T>(url);
                    if (response != null && check(response))
                    {
                        return response;
                    }
                }
                catch (HttpRequestException ex)
                {
                    lastException = ex;
                }

                await Task.Delay(500);
            }

            throw new InvalidOperationException($"Timed out waiting for the client. Last response: {(lastException != null ? lastException.ToString() : JsonSerializer.Serialize(response))}");
        }
    }
}
