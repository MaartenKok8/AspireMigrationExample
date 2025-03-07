using Microsoft.Extensions.Options;
using Shared.Dapr;
using Shared.Events;
using Shared.Extensions;

namespace ApiGateway.Catalog
{
    public static class Services
    {
        public static void AddCatalogServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CatalogServiceOptions>(configuration.GetSection("CatalogService"));
            services.AddHttpClient<CatalogService>((p, httpClient) =>
                {
                    var options = p.GetRequiredService<IOptionsMonitor<CatalogServiceOptions>>();
                    httpClient.BaseAddress = new Uri(options.CurrentValue.BaseUrl);
                })
                .AddRetryPolicy();
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
