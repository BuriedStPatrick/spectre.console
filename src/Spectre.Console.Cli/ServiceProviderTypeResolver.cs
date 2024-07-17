namespace Spectre.Console.Cli;

internal sealed class ServiceProviderTypeResolver : ITypeResolver
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceProviderTypeResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public object? Resolve(Type? type) =>
        type is null
            ? null
            : _serviceProvider.GetService(type!);
}