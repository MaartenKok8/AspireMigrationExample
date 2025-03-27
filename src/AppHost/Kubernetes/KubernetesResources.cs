namespace AppHost.Kubernetes;

public class KubernetesResources
{
    public KubernetesResource Memory { get; } = new KubernetesResource();
    public KubernetesResource Cpu { get; } = new KubernetesResource();
}