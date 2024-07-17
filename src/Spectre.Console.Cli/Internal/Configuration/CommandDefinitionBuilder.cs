using System.Threading;

namespace Spectre.Console.Cli;

/// <summary>
///     Represents a command definition.
/// </summary>
public interface ICommandDefinition
{
    /// <summary>
    ///     Gets the name of the command.
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     Gets aliases for the command.
    /// </summary>
    HashSet<string> Aliases { get; }

    /// <summary>
    ///     Gets examples for the command.
    /// </summary>
    Queue<string[]> Examples { get; }

    /// <summary>
    ///     Gets description of the command.
    /// </summary>
    string? Description { get; }

    /// <summary>
    ///     Gets data for the command.
    /// </summary>
    object? Data { get; }

    /// <summary>
    ///     Gets the type of the command.
    /// </summary>
    Type? CommandType { get; }

    /// <summary>
    ///     Gets a value indicating whether the command is a default command.
    /// </summary>
    bool IsDefaultCommand { get; }

    /// <summary>
    ///     Gets a value indicating whether the command is hidden.
    /// </summary>
    bool IsHidden { get; }
}

/// <summary>
///     Command builder without attached settings.
/// </summary>
/// <typeparam name="TCommandDefinitionBuilder">
///     The <see cref="ICommandDefinitionBuilder{TCommandDefinitionBuilder,TCommandSettings}"/> implementation type.
/// </typeparam>
/// <typeparam name="TCommandDefinition">
///     The <see cref="ICommandDefinition"/> builder.
/// </typeparam>
public interface ICommandDefinitionBuilder<out TCommandDefinitionBuilder, out TCommandDefinition>
    where TCommandDefinitionBuilder : ICommandDefinitionBuilder<TCommandDefinitionBuilder, TCommandDefinition>
    where TCommandDefinition : ICommandDefinition
{
    /// <summary>
    ///     Sets the name of the command.
    /// </summary>
    /// <param name="name">The name of the command.</param>
    /// <returns>The current <typeparamref name="TCommandDefinitionBuilder"/> instance.</returns>
    TCommandDefinitionBuilder WithName(string name);

    /// <summary>
    ///     Adds alias to the command.
    /// </summary>
    /// <param name="alias">The alias to add.</param>
    /// <returns>The current <typeparamref name="TCommandDefinitionBuilder"/> instance.</returns>
    TCommandDefinitionBuilder WithAlias(string alias);

    /// <summary>
    ///     Adds aliases to the command.
    /// </summary>
    /// <param name="aliases">The aliases to add.</param>
    /// <returns>The current <typeparamref name="TCommandDefinitionBuilder"/> instance.</returns>
    TCommandDefinitionBuilder WithAliases(string[] aliases);

    /// <summary>
    ///     Adds examples to the command.
    /// </summary>
    /// <param name="examples">The examples to add.</param>
    /// <returns>The current <typeparamref name="TCommandDefinitionBuilder"/> instance.</returns>
    TCommandDefinitionBuilder WithExample(string[] examples);

    /// <summary>
    ///     Sets the description of the command.
    /// </summary>
    /// <param name="description">The description to set.</param>
    /// <returns>The current <typeparamref name="TCommandDefinitionBuilder"/> instance.</returns>
    TCommandDefinitionBuilder WithDescription(string description);

    /// <summary>
    ///     Sets the command as default.
    /// </summary>
    /// <returns>The current <typeparamref name="TCommandDefinitionBuilder"/> instance.</returns>
    TCommandDefinitionBuilder AsDefault();

    /// <summary>
    ///     Sets the command as hidden.
    /// </summary>
    /// <returns>The current <typeparamref name="TCommandDefinitionBuilder"/> instance.</returns>
    TCommandDefinitionBuilder AsHidden();

    /// <summary>
    ///     Sets data that will be passed to the command via the <see cref="CommandContext"/>.
    /// </summary>
    /// <param name="data">The data to set.</param>
    /// <returns>The current <typeparamref name="TCommandDefinitionBuilder"/> instance.</returns>
    TCommandDefinitionBuilder WithData(object data);

    /// <summary>
    /// Build the command definition.
    /// </summary>
    /// <returns>The built command definition.</returns>
    TCommandDefinition Build();
}

/// <summary>
///     Command builder with attached settings.
/// </summary>
/// <typeparam name="TCommandDefinitionBuilder">
///     The <see cref="ICommandDefinitionBuilder{TCommandDefinitionBuilder,TCommandSettings,TCommandDefinition}"/> implementation type.
/// </typeparam>
/// <typeparam name="TCommandSettings">
///     The <see cref="ICommandSettings"/> implementation type.
/// </typeparam>
/// <typeparam name="TCommandDefinition">
///     The <see cref="ICommandDefinition"/> implementation type.
/// </typeparam>
public interface ICommandDefinitionBuilder<out TCommandDefinitionBuilder, TCommandSettings, out TCommandDefinition> : ICommandDefinitionBuilder<TCommandDefinitionBuilder, TCommandDefinition>
    where TCommandDefinitionBuilder : ICommandDefinitionBuilder<TCommandDefinitionBuilder, TCommandSettings, TCommandDefinition>
    where TCommandSettings : ICommandSettings
    where TCommandDefinition : ICommandDefinition
{
    /// <summary>
    ///     Gets the settings type for the command.
    /// </summary>
    Type SettingsType { get; }

    /// <summary>
    ///     Gets the command's settings.
    /// </summary>
    /// <returns>The settings as the indicated type.</returns>
    TCommandSettings GetSettings();

    /// <summary>
    ///     Sets the settings of the command.
    /// </summary>
    /// <param name="settings">THe settings to set.</param>
    /// <returns>The current <typeparamref name="TCommandDefinitionBuilder"/> instance.</returns>
    TCommandDefinitionBuilder WithSettings(TCommandSettings settings);

    /// <summary>
    ///     Sets the delegate of the command.
    /// </summary>
    /// <param name="delegate">The delegate to set.</param>
    /// <returns>The current <typeparamref name="TCommandDefinitionBuilder"/> instance.</returns>
    TCommandDefinitionBuilder WithAsyncDelegate(Func<CommandContext, TCommandSettings, CancellationToken, Task<int>> @delegate);

    /// <summary>
    ///     Sets the delegate of the command.
    /// </summary>
    /// <param name="delegate">The delegate to set.</param>
    /// <returns>The current <typeparamref name="TCommandDefinitionBuilder"/> instance.</returns>
    TCommandDefinitionBuilder WithDelegate(Func<CommandContext, TCommandSettings, int> @delegate);

    /// <summary>
    /// Gets the command's delegate.
    /// </summary>
    Func<CommandContext, TCommandSettings, CancellationToken, Task<int>>? Delegate { get; }
}

internal sealed class CommandDefinitionBuilder : ICommandDefinitionBuilder<CommandDefinitionBuilder>
{
    private string Name { get; private set; }
    private HashSet<string> Aliases { get; }
    private string? Description { get; private set; }
    private object? Data { get; private set; }
    private Type? CommandType { get; }
    private bool IsDefaultCommand { get; private set; }
    private bool IsHidden { get; private set; }
    private Queue<string[]> Examples { get; }

    public CommandDefinitionBuilder()
    {
        Aliases = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        Examples = new Queue<string[]>();
    }

    public CommandDefinitionBuilder WithName(string name)
    {
        Name = name;

        return this;
    }

    public CommandDefinitionBuilder WithAlias(string alias)
    {
        Aliases.Add(alias);

        return this;
    }

    public CommandDefinitionBuilder WithAliases(string[] aliases)
    {
        foreach (var alias in aliases)
        {
            Aliases.Add(alias);
        }

        return this;
    }

    public CommandDefinitionBuilder WithExample(string[] examples)
    {
        Examples.Enqueue(examples);

        return this;
    }

    public CommandDefinitionBuilder WithDescription(string description)
    {
        Description = description;

        return this;
    }

    public CommandDefinitionBuilder AsDefault()
    {
        IsDefaultCommand = true;

        // Default commands are always created as hidden.
        IsHidden = true;

        return this;
    }

    public CommandDefinitionBuilder AsHidden()
    {
        IsHidden = true;

        return this;
    }

    public CommandDefinitionBuilder WithData(object data)
    {
        Data = data;

        return this;
    }
}

internal sealed class CommandDefinitionBuilder<TCommand, TCommandSettings> : ICommandDefinitionBuilder<
    CommandDefinitionBuilder<TCommand, TCommandSettings>,
    TCommandSettings
>
    where TCommand : ICommand<TCommandSettings>
    where TCommandSettings : ICommandSettings
{
    public string Name { get; private set; }
    public HashSet<string> Aliases { get; }
    public string? Description { get; private set; }
    public object? Data { get; private set; }
    public Type? CommandType { get; private set; }
    public Type? SettingsType { get; private set; }
    public bool IsDefaultCommand { get; private set; }
    public bool IsHidden { get; private set; }
    public IList<ICommandDefinitionBuilder> Children { get; }
    public Queue<string[]> Examples { get; }
    public Func<CommandContext, TCommandSettings, CancellationToken, Task<int>>? Delegate { get; private set; }

    private TCommandSettings? _settings;

    public CommandDefinitionBuilder()
    {
        Aliases = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        Children = new List<ICommandDefinitionBuilder>();
        Examples = new Queue<string[]>();
        SettingsType = typeof(TCommandSettings);
    }

    public TCommandSettings GetSettings() => _settings;

    public CommandDefinitionBuilder<TCommand, TCommandSettings> WithName(string name)
    {
        Name = name;

        return this;
    }

    public CommandDefinitionBuilder<TCommand, TCommandSettings> WithAlias(string alias)
    {
        Aliases.Add(alias);

        return this;
    }

    public CommandDefinitionBuilder<TCommand, TCommandSettings> WithAliases(string[] aliases)
    {
        foreach (var alias in aliases)
        {
            Aliases.Add(alias);
        }

        return this;
    }

    public CommandDefinitionBuilder<TCommand, TCommandSettings> WithExample(string[] examples)
    {
        Examples.Enqueue(examples);

        return this;
    }

    public CommandDefinitionBuilder<TCommand, TCommandSettings> WithDescription(string description)
    {
        Description = description;

        return this;
    }

    public CommandDefinitionBuilder<TCommand, TCommandSettings> WithSettings(TCommandSettings settings)
    {
        _settings = settings;
        SettingsType = typeof(TCommandSettings);

        return this;
    }

    public CommandDefinitionBuilder<TCommand, TCommandSettings> WithAsyncDelegate(Func<CommandContext, TCommandSettings, CancellationToken, Task<int>> @delegate)
    {
        Delegate = @delegate;

        return this;
    }

    public CommandDefinitionBuilder<TCommand, TCommandSettings> WithDelegate(Func<CommandContext, TCommandSettings, int> @delegate)
    {
        Delegate = (context, settings, _) => Task.FromResult(@delegate(context, settings));

        return this;
    }

    public CommandDefinitionBuilder<TCommand, TCommandSettings> AsDefault()
    {
        IsDefaultCommand = true;

        // Default commands are always created as hidden.
        IsHidden = true;

        return this;
    }

    public CommandDefinitionBuilder<TCommand, TCommandSettings> AsHidden()
    {
        IsHidden = true;

        return this;
    }

    public CommandDefinitionBuilder<TCommand, TCommandSettings> WithData(object data)
    {
        Data = data;

        return this;
    }

    public TCommand Build()
    {
        throw new NotImplementedException();
    }
}