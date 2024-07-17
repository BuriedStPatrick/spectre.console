namespace Spectre.Console.Cli;

public sealed class Configurator :
    IConfigurator<Configurator, CommandDefinitionBuilder<,>, BranchConfigurator>,
    IUnsafeConfigurator<Configurator, UnsafeBranchConfigurator>,
    IConfiguration
{
    private readonly ITypeRegistrar _typeRegistrar;
    public IList<ICommandDefinitionBuilder> Commands { get; }
    public ICommandAppSettings Settings { get; }
    public ICommandDefinitionBuilder? DefaultCommand { get; }
    public IList<string[]> Examples { get; }

    public Configurator(ITypeRegistrar typeRegistrar)
    {
        _typeRegistrar = typeRegistrar;

        Commands = new List<ICommandDefinitionBuilder>();
        Settings = new CommandAppSettings(_typeRegistrar);
        Examples = new List<string[]>();
    }

    public Configurator WithHelpProvider<THelpProvider>(THelpProvider helpProvider)
        where THelpProvider : IHelpProvider
    {
        _typeRegistrar.RegisterInstance(helpProvider);

        return this;
    }

    public Configurator WithHelpProvider<THelpProvider>()
        where THelpProvider : IHelpProvider
    {
        _typeRegistrar.Register(typeof(IHelpProvider), typeof(THelpProvider));

        return this;
    }

    public Configurator WithExample(params string[] args)
    {
        Examples.Add(args);

        return this;
    }

    public Configurator AddCommand<TCommand>(
        string name,
        Action<CommandDefinitionBuilder<,>>? configureCommand = null)
        where TCommand : class, ICommand
    {
        var builder = new CommandDefinitionBuilder<,>();

        configureCommand?.Invoke(builder);

        var configuredCommand = new CommandDefinitionBuilder<,>().FromType<TCommand>(name);

        var configurator = new CommandConfigurator(configuredCommand);

        configureCommand?.Invoke(configurator);

        Commands.Add(configuredCommand);

        return this;
    }

    public Configurator AddDelegate<TCommandSettings>(
        string name,
        Func<CommandContext, TCommandSettings, int> func,
        Action<CommandConfigurator>? configureCommand = null)
        where TCommandSettings : ICommandSettings
    {
        var configuredCommand = CommandDefinitionBuilder<,>.FromDelegate(name, func);

        return this;
    }

    public Configurator AddAsyncDelegate<TSettings>(string name, Func<CommandContext, TSettings, Task<int>> func, Action<CommandConfigurator>? configureCommand = null) where TSettings : ICommandSettings
    {
        throw new NotImplementedException();
        return this;
    }

    public Configurator AddBranch<TBranchSettings>(string name, Action<BranchConfigurator> configureBranch) where TBranchSettings : ICommandSettings
    {
        throw new NotImplementedException();
        return this;
    }

    public Configurator AddBranch<TBranchSettings>(string name, Action<TBranchSettings> configureBranch) where TBranchSettings : ICommandSettings
    {
        throw new NotImplementedException();
        return this;
    }

    public Configurator AddCommand(string name, Type command, Action<CommandConfigurator>? configureCommand = null)
    {
        throw new NotImplementedException();
        return this;
    }

    public Configurator AddBranch(string name, Type settings, Action<UnsafeBranchConfigurator> configureBranch)
    {
        throw new NotImplementedException();
        return this;
    }
}