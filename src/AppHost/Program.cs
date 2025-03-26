using CommunityToolkit.Aspire.Hosting.Dapr;

var builder = DistributedApplication.CreateBuilder(args);

var rabbitMq = builder
    .AddRabbitMQ("rabbitmq",
        userName: builder.AddParameter("RabbitMqUserName", () => "guest"),
        password: builder.AddParameter("RabbitMqPassword", () => "guest", secret: true),
        port: 5672)
    .WithLifetime(ContainerLifetime.Persistent);

var mongo = builder
    .AddMongoDB("mongodb", 
        port: 27017,
        userName: builder.AddParameter("MongoUserName", () => "user"),
        password: builder.AddParameter("MongoPassword", () => "password", secret: true))
    .WithLifetime(ContainerLifetime.Persistent);

var pubSub = builder.AddDaprPubSub("pub-sub", new DaprComponentOptions
{
    LocalPath = Path.Combine("..", "..", "config", "dapr", "components", "pub-sub.yml")
}).WaitFor(rabbitMq);

var stateStore = builder.AddDaprStateStore("state-store", new DaprComponentOptions
{
    LocalPath = Path.Combine("..", "..", "config", "dapr", "components", "state-store.yml")
}).WaitFor(mongo);


var catalogService = builder
    .AddProject<Projects.CatalogService>("catalogservice")
    .WithEnvironment("Dapr__PubSub", pubSub.Resource.Name)
    .WithEnvironment("Dapr__StateStore", stateStore.Resource.Name)
    .WithReference(pubSub)
    .WithReference(stateStore)
    .WithDaprSidecar();

builder
    .AddProject<Projects.ApiGateway>("apigateway")
    .WithReference(catalogService)
    .WithEnvironment("Dapr__PubSub", pubSub.Resource.Name)
    .WithReference(pubSub)
    .WithDaprSidecar();

builder.Build().Run();