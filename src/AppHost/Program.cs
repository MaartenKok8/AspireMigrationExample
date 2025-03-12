var builder = DistributedApplication.CreateBuilder(args);

var catalogService = builder
    .AddProject<Projects.CatalogService>("catalogservice")
    .WithEnvironment("Dapr__PubSub", "pub-sub")
    .WithEnvironment("Dapr__StateStore", "state-store");

builder
    .AddProject<Projects.ApiGateway>("apigateway")
    .WithReference(catalogService)
    .WithEnvironment("Dapr__PubSub", "pub-sub");

builder.Build().Run();