namespace AppHost.Kubernetes;

public class KubernetesOptions
{
    public int Replicas { get; set; } = 1;
    
    public KubernetesResources Resources { get; } = new KubernetesResources();
}