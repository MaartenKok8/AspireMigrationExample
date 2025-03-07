using System.Net.Http.Json;
using System.Text.Json;

namespace IntegrationTests
{
    public class IntegrationTest
    {
        protected static HttpClient GetGatewayClient() => new HttpClient
        {
            BaseAddress = new Uri("http://localhost:4000")
        };

        protected Task<T> PollGatewayUntilAsync<T>(Func<T, bool> check, string url) => PollUntilAsync(GetGatewayClient(), check, url);

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
