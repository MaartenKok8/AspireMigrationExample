var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ApiGateway>("apigateway");

builder.AddProject<Projects.CatalogService>("catalogservice");

builder.Build().Run();