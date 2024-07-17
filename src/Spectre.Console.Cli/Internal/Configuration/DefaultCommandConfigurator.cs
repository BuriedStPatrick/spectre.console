namespace Spectre.Console.Cli.Internal.Configuration;

/// <summary>
/// Fluent configurator for the default command.
/// </summary>
public sealed class DefaultCommandConfigurator
{
    private readonly CommandDefinitionBuilder<,> _defaultCommandDefinitionBuilder;

    internal DefaultCommandConfigurator(CommandDefinitionBuilder<,> defaultCommandDefinitionBuilder)
    {
        _defaultCommandDefinitionBuilder = defaultCommandDefinitionBuilder;
    }

    /// <summary>
    /// Sets the description of the default command.
    /// </summary>
    /// <param name="description">The default command description.</param>
    /// <returns>The same <see cref="DefaultCommandConfigurator"/> instance so that multiple calls can be chained.</returns>
    public DefaultCommandConfigurator WithDescription(string description)
    {
        _defaultCommandDefinitionBuilder.Description = description;
        return this;
    }

    /// <summary>
    /// Sets data that will be passed to the command via the <see cref="CommandContext"/>.
    /// </summary>
    /// <param name="data">The data to pass to the default command.</param>
    /// <returns>The same <see cref="DefaultCommandConfigurator"/> instance so that multiple calls can be chained.</returns>
    public DefaultCommandConfigurator WithData(object data)
    {
        _defaultCommandDefinitionBuilder.Data = data;
        return this;
    }
}