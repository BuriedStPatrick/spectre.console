namespace Spectre.Console.Cli;

internal sealed class Configurator<TDefaultCommandSettings> :
    IConfigurator<
        Configurator<TDefaultCommandSettings>,
        TDefaultCommandSettings,
        CommandConfigurator,
        BranchConfigurator
    >,
    IUnsafeConfigurator<
        Configurator<TDefaultCommandSettings>,
        UnsafeBranchConfigurator,
        CommandConfigurator
    >
    where TDefaultCommandSettings : ICommandSettings
{
    private readonly CommandDefinitionBuilder<,> _commandDefinitionBuilder;
    private readonly ITypeRegistrar? _registrar;

    public Configurator(CommandDefinitionBuilder<,> commandDefinitionBuilder, ITypeRegistrar? registrar)
    {
        _commandDefinitionBuilder = commandDefinitionBuilder;
        _registrar = registrar;
    }

    public Configurator<TDefaultCommandSettings> WithDefaultCommand<TDefaultCommand>()
        where TDefaultCommand : class, ICommand
    {
        var defaultCommand = new CommandDefinitionBuilder<,>()
            .WithName(CliConstants.DefaultCommandName)
            .AsDefault();

        _commandDefinitionBuilder.Children.Add(defaultCommand);

        return this;
    }

    public Configurator<TDefaultCommandSettings> WithHiddenBranch()
    {
        _commandDefinitionBuilder.AsHidden();

        return this;
    }

    public Configurator<TDefaultCommandSettings> AddCommand<TCommand, TCommandSettings>(
        string name,
        Action<CommandConfigurator>? configureCommand = null)
        where TCommand : ICommand<TCommandSettings>
        where TCommandSettings : ICommandSettings
    {
        var command = new CommandDefinitionBuilder<,>()
            .WithName(name)
            .WithCommandType<TCommand>();

        var configurator = new CommandConfigurator(command);

        configureCommand?.Invoke(configurator);

        _commandDefinitionBuilder.Children.Add(configurator.CommandBuilder);

        return this;
    }

    public Configurator<TDefaultCommandSettings> AddDelegate<TDerivedSettings>(
        string name,
        Func<CommandContext, TDerivedSettings, int> func,
        Action<CommandConfigurator>? configureCommand = null)
        where TDerivedSettings : ICommandSettings
    {
        var command = CommandDefinitionBuilder<,>.FromAsyncDelegate<TDerivedSettings>(
            name, (context, settings) => Task.FromResult(func(context, (TDerivedSettings)settings)));

        var configurator = new CommandConfigurator(command);
        configureCommand?.Invoke(configurator);

        _commandDefinitionBuilder.Children.Add(configurator.CommandBuilder);

        return this;
    }

    public Configurator<TDefaultCommandSettings> AddAsyncDelegate<TDerivedSettings>(
        string name,
        Func<CommandContext, TDerivedSettings, Task<int>> func,
        Action<CommandConfigurator>? configureCommand = null)
        where TDerivedSettings : TDefaultCommandSettings
    {
        var command = CommandDefinitionBuilder<,>.FromAsyncDelegate<TDerivedSettings>(
            name, (context, settings) => func(context, (TDerivedSettings)settings));

        var configurator = new CommandConfigurator(command);

        _commandDefinitionBuilder.Children.Add(configurator.CommandBuilder);

        return this;
    }

    public Configurator<TDefaultCommandSettings> AddBranch<TDerivedSettings>(
        string name,
        Action<BranchConfigurator> configureBranch)
        where TDerivedSettings : ICommandSettings
    {
        var command = CommandDefinitionBuilder<,>.FromBranch<TDerivedSettings>(name);
        var configurator = new BranchConfigurator(command);

        configureBranch(configurator);

        return this;
    }

    public Configurator<TDefaultCommandSettings> AddCommand<TCommandConfigurator>(string name, Type command, Action<TCommandConfigurator>? configureCommand = null)
        where TCommandConfigurator : ICommandConfigurator<TCommandConfigurator>
    {
        var method = GetType().GetMethod("AddCommand");
        if (method == null)
        {
            throw new CommandConfigurationException("Could not find AddCommand by reflection.");
        }

        method = method.MakeGenericMethod(command);

        if (method.Invoke(this, [name]) is not TCommandConfigurator result)
        {
            throw new CommandConfigurationException("Invoking AddCommand returned null.");
        }

        configureCommand?.Invoke(result);

        return this;
    }

    public Configurator<TDefaultCommandSettings> AddCommand(string name, Type command, Action<CommandConfigurator>? configureCommand = null)
    {
        var configuredCommand = CommandDefinitionBuilder<,>.FromType(
            command,
            name,
            isDefaultCommand: false);

        var commandConfigurator = new CommandConfigurator(configuredCommand);

        configureCommand?.Invoke(commandConfigurator);

        _commandDefinitionBuilder.Children.Add(configuredCommand);

        return this;
    }

    public Configurator<TDefaultCommandSettings> AddBranch(string name, Type settings, Action<UnsafeBranchConfigurator> configureBranch)
    {
        var command = CommandDefinitionBuilder<,>.FromBranch(settings, name);

        // Create the configurator.
        var configuratorType = typeof(Configurator<>).MakeGenericType(settings);
        if (Activator.CreateInstance(configuratorType, [command, _registrar]) is not UnsafeBranchConfigurator configurator)
        {
            throw new CommandConfigurationException("Could not create configurator by reflection.");
        }

        configureBranch(configurator);

        _commandDefinitionBuilder.Children.AddAndReturn(command);

        return this;
    }
}