using AppHost.Kubernetes;

namespace AppHost.Extensions;

public static class ProjectResourceBuilderExtensions
{
    public static IResourceBuilder<ProjectResource> PublishToKubernetes(this IResourceBuilder<ProjectResource> project, Action<KubernetesOptions> configureOptions)
    {
        if (!project.ApplicationBuilder.ExecutionContext.IsPublishMode)
            return project;
        
        var options = new KubernetesOptions();
        configureOptions(options);

        project.WithEnvironment("ASPIRE_REPLICAS", options.Replicas.ToString());
        project.WithEnvironment("ASPIRE_MEMORY_REQUEST", options.Resources.Memory.Request);
        project.WithEnvironment("ASPIRE_MEMORY_LIMIT", options.Resources.Memory.Limit);
        project.WithEnvironment("ASPIRE_CPU_REQUEST", options.Resources.Cpu.Request);
        project.WithEnvironment("ASPIRE_CPU_LIMIT", options.Resources.Cpu.Request);
        
        return project;
    }
}