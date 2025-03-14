using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace IntegrationTests;

[CollectionDefinition(nameof(DistributedApplicationCollection))]
public class DistributedApplicationCollection : ICollectionFixture<DistributedApplicationFixture>
{
    
}

public class DistributedApplicationFixture
{
    private DistributedApplication? distributedApp;
    private readonly SemaphoreSlim initializationLock = new SemaphoreSlim(1);
    
    public async Task<DistributedApplication> GetDistributedApplicationAsync(ITestOutputHelper output, params string[] waitForResources)
    {
        await initializationLock.WaitAsync();

        try
        {
            distributedApp ??= await InitializeDistributedApplicationAsync(output);
            return distributedApp;
        }
        finally
        {
            initializationLock.Release();
        }
    }

    private static async Task<DistributedApplication> InitializeDistributedApplicationAsync(ITestOutputHelper output)
    {
        var builder = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.AppHost>(
                args:
                [
                    "DcpPublisher:RandomizePorts=false" // Important, so that the ports used for RabbitMQ and Mongo stay the same, required because of Dapr
                ]);

        builder.Services.AddLogging(b => b.AddXUnit(output));
        var distributedApp = builder.Build();
        await distributedApp.StartAsync();
        
        var resources = distributedApp.Services.GetRequiredService<ResourceNotificationService>();
        await resources.WaitForResourceHealthyAsync("apigateway-dapr-cli") // Note we are waiting for the dapr sidecar to become active
            .WaitAsync(TimeSpan.FromSeconds(120));
        await resources.WaitForResourceHealthyAsync("catalogservice-dapr-cli")
            .WaitAsync(TimeSpan.FromSeconds(120));
        
        await Task.Delay(5_000); // Wait an additional 5 seconds, as when the Dapr sidecar is started, the pub-sub subscriptions are not created yet.
        
        return distributedApp;
    }
}