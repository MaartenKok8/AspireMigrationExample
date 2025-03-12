var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ApiGateway>("apigateway");

builder.AddProject<Projects.CatalogService>("catalogservice")
    .WithEnvironment("Dapr__PubSub", "pub-sub")
    .WithEnvironment("Dapr__StateStore", "state-store");

builder.Build().Run();