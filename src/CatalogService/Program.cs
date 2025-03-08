using CatalogService;
using CatalogService.Extensions;
using Shared.Dapr;
using Shared.Events;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddSharedServices();

var app = builder.Build();

app.MapDefaultEndpoints();

app.AddSharedEndpoints();
app.MapEventHandler<ProductAddedEvent>(async (ProductAddedEvent @event, IStateStore stateStore) =>
{
    // Some validation

    var entity = @event.ToEntity();
    await stateStore.SaveStateAsync($"product_{@event.Id}", entity);
});

app.MapGet("products/{Id}", async (string id, IStateStore stateStore) =>
{
    var product = await stateStore.GetStateAsync<ProductEntity>($"product_{id}");

    return product == null
        ? Results.NotFound($"A product with id '{id}' was not found.")
        : Results.Ok(product);
});

app.Run();
