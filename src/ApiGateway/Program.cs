using ApiGateway.Catalog;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddCatalogServices(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

app.AddCatalogEndpoints();

app.Run();