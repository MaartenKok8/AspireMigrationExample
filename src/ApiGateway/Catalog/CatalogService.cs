using System.Text.Json;

namespace ApiGateway.Catalog
{
    public class CatalogService(HttpClient httpClient)
    {
        public async Task<JsonElement> GetProductAsync(string id)
        {
            return await httpClient.GetFromJsonAsync<JsonElement>($"products/{id}");
        }
    }
}
