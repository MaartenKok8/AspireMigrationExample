using Shared.Dapr;
using Shared.Events;

namespace ApiGateway.Catalog
{
    public static class Services
    {
        public static void AddCatalogServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<CatalogService>(httpClient => httpClient.BaseAddress = new Uri("http+https://catalogservice"));
        }

        public static void AddCatalogEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("products");
            group.MapGet("{Id}", async (string id, CatalogService service) =>
            {
                // TODO better error handling, as 404's get translated to 500's because of unhandled exceptions.
                return await service.GetProductAsync(id);
            });

            group.MapPost("", async (ProductAddedEvent @event, IPubSub pubSub) =>
            {
                await pubSub.PublishEventAsync(@event);
            });
        }
    }
}
