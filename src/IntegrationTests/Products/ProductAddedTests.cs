using System.Net.Http.Json;

namespace IntegrationTests.Products
{
    public class ProductAddedTests : IntegrationTest
    {
        [Fact]
        public async Task Publishing_product_added_event_eventually_makes_product_available_async()
        {
            // Create a unique product id, so that tests and/or test runs don't interfere with eachother
            var productId = Guid.NewGuid().ToString();
            var gateway = GetGatewayClient();

            // Act
            var response = await gateway.PostAsJsonAsync("products", new
            {
                id = productId,
                description = "This is a product description",
                price = 1.24
            });

            response.EnsureSuccessStatusCode();

            // Assert

            // Try retrieving the product until it is available
            var product = await PollGatewayUntilAsync<Product>(p => true, $"products/{productId}");
            Assert.Equal(
                new Product(productId, "This is a product description", 1.24M),
                product);
        }

        record Product(string Id, string Description, decimal Price);
    }
}
