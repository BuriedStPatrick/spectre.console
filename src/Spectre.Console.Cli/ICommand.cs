namespace Spectre.Console.Cli;

/// <summary>
/// Represents a command.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Validates the specified settings and remaining arguments.
    /// </summary>
    /// <param name="context">The command context.</param>
    /// <param name="settings">The settings.</param>
    /// <returns>The validation result.</returns>
    ValidationResult Validate(CommandContext context, ICommandSettings settings);

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="context">The command context.</param>
    /// <param name="settings">The settings.</param>
    /// <returns>The validation result.</returns>
    Task<int> Execute(CommandContext context, ICommandSettings settings);
}

/// <summary>
/// Represents a command with assigned <see cref="ICommandSettings"/>.
/// </summary>
/// <typeparam name="TCommandSettings">The settings type.</typeparam>
public interface ICommand<in TCommandSettings> : ICommand
    where TCommandSettings : ICommandSettings
{
    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="context">The command context.</param>
    /// <param name="settings">The settings.</param>
    /// <returns>An integer indicating whether the command executed successfully.</returns>
    Task<int> Execute(CommandContext context, TCommandSettings settings);
}