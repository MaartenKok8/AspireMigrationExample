using ApiGateway.Catalog;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddSharedServices();
builder.Services.AddCatalogServices(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

app.AddSharedEndpoints();
app.AddCatalogEndpoints();

app.Run();