using Microsoft.Extensions.DependencyInjection;

namespace Spectre.Console.Cli;

internal sealed class ServiceCollectionTypeRegistrar : ITypeRegistrar
{
    private readonly IServiceCollection _serviceCollection;

    public ServiceCollectionTypeRegistrar(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public void Register(Type service, Type implementation)
    {
        _serviceCollection.AddScoped(service, implementation);
    }

    public void RegisterInstance(Type service, object implementation)
    {
        _serviceCollection.AddScoped(service, implementation.GetType());
    }

    public void RegisterInstance<TImplementation>(TImplementation implementation)
        where TImplementation : class
    {
        _serviceCollection.AddScoped<TImplementation>(_ => implementation);
    }

    public void RegisterLazy(Type service, Func<object> factory)
    {
        _serviceCollection.AddScoped(service, _ => factory);
    }

    public ITypeResolver Build() =>
        new ServiceProviderTypeResolver(
            _serviceCollection.BuildServiceProvider()
                .CreateScope()
                .ServiceProvider);
}