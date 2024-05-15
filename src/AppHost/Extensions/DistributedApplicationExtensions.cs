namespace AppHost.Extensions;

public static class DistributedApplicationExtensions
{
    public static IResourceBuilder<ParameterResource> CreateParameter(this IDistributedApplicationBuilder builder, string name, string value)
    {
        var resource = new ParameterResource(name, (_) => value);
        return builder.AddResource(resource);
    }
}