using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Dapr;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Events;

namespace Shared.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static void AddSharedEndpoints(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Dapr subscription
            app.MapSubscribeHandler();
            app.UseCloudEvents();
        }

        public static RouteHandlerBuilder MapEventHandler<TEvent>(this IEndpointRouteBuilder builder, Delegate requestDelegate) where TEvent : IEvent
        {
            var daprOptions = builder.ServiceProvider.GetRequiredService<IOptionsMonitor<DaprOptions>>().CurrentValue;

            return builder
                .MapPost($"events/{TEvent.Topic}", requestDelegate)
                .WithTopic(daprOptions.PubSub, TEvent.Topic, new Dictionary<string, string?>() { { "queueType", "quorum" } })
                .AddEndpointFilterFactory((factoryContext, next) =>
                {
                    var logger = factoryContext.ApplicationServices.GetRequiredService<ILogger<IEndpointRouteBuilder>>();

                    return async context =>
                    {
                        logger.LogInformation("Processing event on topic '{Topic}'.", TEvent.Topic);
                        return await next(context);
                    };
                });
        }
    }
}
