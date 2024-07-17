using System.Threading;

namespace Spectre.Console.Cli;

/// <summary>
/// Represents a command line application.
/// </summary>
public interface ICommandApp
{
    /// <summary>
    /// Runs the command line application with specified arguments.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>The exit code from the executed command.</returns>
    int Run(string[] args);

    /// <summary>
    /// Runs the command line application with specified arguments.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <param name="cancellationToken">Cancellation token for cancelling the run.</param>
    /// <returns>The exit code from the executed command.</returns>
    Task<int> RunAsync(
        string[] args,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Represents a command line application.
/// </summary>
/// <typeparam name="TCommandApp">The <see cref="ICommandApp{TCommandApp}"/> implementation.</typeparam>
/// <typeparam name="TConfigurator">The <see cref="IConfigurator{TCommandConfigurator,TDefaultCommandSettings,TConfigurator}"/> implementation.</typeparam>
public interface ICommandApp<out TCommandApp, out TConfigurator>
    where TCommandApp : ICommandApp<TCommandApp, TConfigurator>
{
    /// <summary>
    /// Configures the command line application.
    /// </summary>
    /// <param name="configureConfigurator">The <typeparamref name="TCommandApp"/> configuration delegate.</param>
    /// <returns>The <typeparamref name="TCommandApp"/> instance.</returns>
    TCommandApp Configure(Action<TConfigurator> configureConfigurator);
}

/// <summary>
/// Represents a command line application.
/// </summary>
/// <typeparam name="TCommandApp">The <see cref="ICommandApp{TCommandApp}"/> implementation.</typeparam>
public interface ICommandApp<out TCommandApp> : ICommandApp
    where TCommandApp : ICommandApp<TCommandApp>
{
    /// <summary>
    /// Configures the command line application.
    /// </summary>
    /// <param name="configureApp">The <typeparamref name="TCommandApp"/> configuration delegate.</param>
    /// <returns>The <typeparamref name="TCommandApp"/> instance.</returns>
    /// <typeparam name="TConfigurator">The <see cref="IConfigurator{TCommandConfigurator,TDefaultCommandSettings,TConfigurator}"/> implementation type.</typeparam>
    /// <typeparam name="TCommandConfigurator">The <see cref="ICommandConfigurator{TCommandConfigurator}"/> implementation type.</typeparam>
    /// <typeparam name="TBranchConfigurator">The <see cref="IBranchConfigurator{TBranchConfigurator}"/> implementation type.</typeparam>
    TCommandApp Configure<TConfigurator, TCommandConfigurator, TBranchConfigurator>(Action<TConfigurator> configureApp)
        where TConfigurator : IConfigurator<TConfigurator, TCommandConfigurator, TBranchConfigurator>
        where TCommandConfigurator : ICommandConfigurator<TCommandConfigurator>
        where TBranchConfigurator : IBranchConfigurator<TBranchConfigurator>;
}