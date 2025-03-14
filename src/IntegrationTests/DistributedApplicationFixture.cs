using Aspire.Hosting;
using Aspire.Hosting.Testing;

namespace IntegrationTests;

[CollectionDefinition(nameof(DistributedApplicationCollection))]
public class DistributedApplicationCollection : ICollectionFixture<DistributedApplicationFixture>
{
    
}

public class DistributedApplicationFixture
{
    private DistributedApplication? distributedApp;
    private readonly SemaphoreSlim initializationLock = new SemaphoreSlim(1);
    
    public async Task<DistributedApplication> GetDistributedApplicationAsync()
    {
        await initializationLock.WaitAsync();

        try
        {
            distributedApp ??= await InitializeDistributedApplicationAsync();
            return distributedApp;
        }
        finally
        {
            initializationLock.Release();
        }
    }

    private static async Task<DistributedApplication> InitializeDistributedApplicationAsync()
    {
        var builder = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.AppHost>(
                args:
                [
                    "DcpPublisher:RandomizePorts=false" // Important, so that the ports used for RabbitMQ and Mongo stay the same, required because of Dapr
                ]);

        var distributedApp = builder.Build();

        await distributedApp.StartAsync();

        return distributedApp;
    }
}