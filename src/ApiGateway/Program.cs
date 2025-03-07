using ApiGateway.Catalog;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddSharedServices();
builder.Services.AddCatalogServices(builder.Configuration);

var app = builder.Build();

app.AddSharedEndpoints();
app.AddCatalogEndpoints();

app.Run();