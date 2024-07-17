using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli.Internal.Configuration;

namespace Spectre.Console.Cli;

public static class Test
{
    public static async Task<int> DoIt()
    {
        var args = new[] { "asd", "asd" };
        var services = new ServiceCollection();

        var app = new CommandLineAppBuilder()
            .WithServiceCollectionTypeRegistrar(services)
            .WithDefaults()

            // Same as just propagating the exception. No need for an additional setting
            .WithExceptionHandler((exception, _) => throw exception)

            // Build the app
            .Build();

        return await app.RunAsync(
            args,
            CancellationToken.None);
    }
}

public sealed class CommandLineApp
{
    private readonly ICommandAppSettings _commandAppSettings;
    private readonly ITypeRegistrar? _typeRegistrar;
    private readonly CommandLineAsyncExceptionHandler? _exceptionHandler;

    public CommandLineApp(
        ICommandAppSettings commandAppSettings,
        ITypeRegistrar? typeRegistrar,
        CommandLineAsyncExceptionHandler? exceptionHandler)
    {
        _commandAppSettings = commandAppSettings;
        _typeRegistrar = typeRegistrar;
        _exceptionHandler = exceptionHandler;
    }

    public async Task<int> RunAsync(string[] args, CancellationToken cancellationToken)
    {
        // TODO: implement
        return 0;
    }
}

public interface ICommandLineExceptionHandler
{
    void Handle(Exception exception);
}

public interface ICommandLineAsyncExceptionHandler
{
    Task Handle(Exception exception, CancellationToken cancellationToken = default);
}

public delegate void CommandLineExceptionHandler(
    Exception exception,
    ITypeResolver? typeResolver);

public delegate Task CommandLineAsyncExceptionHandler(
    Exception exception,
    ITypeResolver? typeResolver,
    CancellationToken cancellationToken = default);

public sealed class CommandLineAppBuilder
{
    private Action<ICommandAppSettings>? _configureCommandApp;
    private CommandLineAsyncExceptionHandler? _asyncExceptionHandler;
    private ITypeRegistrar? _typeRegistrar;
    private Action<ITypeRegistrar>? _configureTypeRegistrar;

    public CommandLineAppBuilder ConfigureApp(Action<ICommandAppSettings> configureCommandApp)
    {
        _configureCommandApp += configureCommandApp;

        return this;
    }

    public CommandLineAppBuilder WithExceptionHandler(CommandLineExceptionHandler exceptionHandler)
    {
        _asyncExceptionHandler += (exception, resolver, _) =>
        {
            exceptionHandler(exception, resolver);

            return Task.CompletedTask;
        };

        return this;
    }

    public CommandLineAppBuilder WithAsyncExceptionHandler(CommandLineAsyncExceptionHandler exceptionHandler)
    {
        _asyncExceptionHandler += exceptionHandler;

        return this;
    }

    public CommandLineAppBuilder WithExceptionHandler<TExceptionHandler>()
        where TExceptionHandler : ICommandLineExceptionHandler
    {
        ConfigureTypeRegistrar(registrar
            => registrar.Register(typeof(TExceptionHandler), typeof(TExceptionHandler)));

        _asyncExceptionHandler += (exception, resolver, _) =>
        {
            if (resolver is not null)
            {
                var handler = resolver.Resolve<TExceptionHandler>();

                handler?.Handle(exception);
            }

            return Task.CompletedTask;
        };

        return this;
    }

    public CommandLineAppBuilder WithAsyncExceptionHandler<TAsyncExceptionHandler>()
        where TAsyncExceptionHandler : ICommandLineAsyncExceptionHandler
    {
        ConfigureTypeRegistrar(registrar
            => registrar.Register(typeof(TAsyncExceptionHandler), typeof(TAsyncExceptionHandler)));

        _asyncExceptionHandler += async (exception, resolver, cancellationToken) =>
        {
            if (resolver is not null)
            {
                var handler = resolver.Resolve<TAsyncExceptionHandler>();

                if (handler is not null)
                {
                    await handler.Handle(exception, cancellationToken);
                }
            }
        };

        return this;
    }

    public CommandLineAppBuilder ConfigureTypeRegistrar(Action<ITypeRegistrar> configureTypeRegistrar)
    {
        _configureTypeRegistrar += configureTypeRegistrar;

        return this;
    }

    public CommandLineAppBuilder WithTypeRegistrar(ITypeRegistrar typeRegistrar)
    {
        _typeRegistrar = typeRegistrar;

        return this;
    }

    internal CommandLineApp Build()
    {
        var commandAppSettings = new CommandAppSettings();
        _configureCommandApp?.Invoke(commandAppSettings);

        if (_typeRegistrar is not null)
        {
            _configureTypeRegistrar?.Invoke(_typeRegistrar);
        }

        return new CommandLineApp(
            commandAppSettings,
            _typeRegistrar,
            _asyncExceptionHandler);
    }
}

public static class CommandLineAppBuilderExtensions
{
    public static CommandLineAppBuilder WithDefaults(this CommandLineAppBuilder commandLineAppBuilder) =>
        commandLineAppBuilder.ConfigureApp(app =>
        {
            app.Console = AnsiConsole.Console;
            app.CaseSensitivity = CaseSensitivity.None;
        });

    public static CommandLineAppBuilder WithServiceCollectionTypeRegistrar(
        this CommandLineAppBuilder commandLineAppBuilder,
        IServiceCollection serviceCollection) =>
        commandLineAppBuilder.WithTypeRegistrar(new ServiceCollectionTypeRegistrar(serviceCollection));
}

/// <summary>
/// The entry point for a command line application.
/// </summary>
public sealed class CommandApp : ICommandApp<CommandApp, Configurator>
{
    private readonly Configurator _configurator;
    private readonly CommandExecutor _executor;
    private bool _executed;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandApp"/> class.
    /// </summary>
    /// <param name="registrar">The registrar.</param>
    public CommandApp(ITypeRegistrar? registrar = null)
    {
        registrar ??= new DefaultTypeRegistrar();

        _configurator = new Configurator(registrar);
        _executor = new CommandExecutor(registrar);
    }

    /// <inheritdoc />
    public CommandApp Configure(Action<Configurator> configureConfigurator)
    {
        configureConfigurator(_configurator);

        return this;
    }

    /// <summary>
    /// Sets the default command.
    /// </summary>
    /// <typeparam name="TCommand">The command type.</typeparam>
    /// <returns>A <see cref="DefaultCommandConfigurator"/> that can be used to configure the default command.</returns>
    public DefaultCommandConfigurator SetDefaultCommand<TCommand>()
        where TCommand : class, ICommand
    {
        return new DefaultCommandConfigurator(GetConfigurator().SetDefaultCommand<TCommand>());
    }

    /// <summary>
    /// Runs the command line application with specified arguments.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>The exit code from the executed command.</returns>
    public int Run(string[] args)
    {
        return RunAsync(args).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Runs the command line application with specified arguments.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>The exit code from the executed command.</returns>
    public async Task<int> RunAsync(string[] args, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!_executed)
            {
                // Add built-in (hidden) commands.
                _configurator.AddBranch(CliConstants.Commands.Branch, cli =>
                {
                    cli.HideBranch();
                    cli.AddCommand<VersionCommand>(CliConstants.Commands.Version);
                    cli.AddCommand<XmlDocCommand>(CliConstants.Commands.XmlDoc);
                    cli.AddCommand<ExplainCommand>(CliConstants.Commands.Explain);
                });

                _executed = true;
            }

            return await _executor
                .Execute(_configurator, args)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // Should we always propagate when debugging?
            if (Debugger.IsAttached
                && ex is CommandAppException appException
                && appException.AlwaysPropagateWhenDebugging)
            {
                throw;
            }

            if (_configurator.Settings.PropagateExceptions)
            {
                throw;
            }

            if (_configurator.Settings.ExceptionHandler != null)
            {
                return _configurator.Settings.ExceptionHandler(ex, null);
            }

            // Render the exception.
            var pretty = GetRenderableErrorMessage(ex);
            if (pretty != null)
            {
                _configurator.Settings.Console.SafeRender(pretty);
            }

            return -1;
        }
    }

    internal Configurator GetConfigurator() => _configurator;

    private static List<IRenderable?>? GetRenderableErrorMessage(Exception ex, bool convert = true)
    {
        if (ex is CommandAppException renderable && renderable.Pretty != null)
        {
            return new List<IRenderable?> { renderable.Pretty };
        }

        if (convert)
        {
            var converted = new List<IRenderable?>
                {
                    new Composer()
                        .Text("[red]Error:[/]")
                        .Space()
                        .Text(ex.Message.EscapeMarkup())
                        .LineBreak(),
                };

            // Got a renderable inner exception?
            if (ex.InnerException != null)
            {
                var innerRenderable = GetRenderableErrorMessage(ex.InnerException, convert: false);
                if (innerRenderable != null)
                {
                    converted.AddRange(innerRenderable);
                }
            }

            return converted;
        }

        return null;
    }
}