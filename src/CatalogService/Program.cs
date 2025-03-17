using CatalogService;
using CatalogService.Extensions;
using ServiceDefaults;
using ServiceDefaults.Dapr;
using ServiceDefaults.Events;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var app = builder.Build();

app.MapDefaultEndpoints();

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
