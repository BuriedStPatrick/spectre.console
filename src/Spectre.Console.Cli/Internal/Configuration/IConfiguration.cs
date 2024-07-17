namespace Spectre.Console.Cli;

/// <summary>
/// Represents a configuration.
/// </summary>
internal interface IConfiguration
{
    /// <summary>
    /// Gets the configured commands.
    /// </summary>
    IList<ICommandDefinitionBuilder> Commands { get; }

    /// <summary>
    /// Gets the settings for the configuration.
    /// </summary>
    ICommandAppSettings Settings { get; }

    /// <summary>
    /// Gets the default command for the configuration.
    /// </summary>
    ICommandDefinitionBuilder? DefaultCommand { get; }

    /// <summary>
    /// Gets all examples for the configuration.
    /// </summary>
    IList<string[]> Examples { get; }
}