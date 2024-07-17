namespace Spectre.Console.Cli;

/// <summary>
/// Base class for an asynchronous command.
/// </summary>
/// <typeparam name="TCommandSettings">The settings type.</typeparam>
public abstract class AsyncCommand<TCommandSettings> : ICommand<TCommandSettings>
    where TCommandSettings : ICommandSettings
{
    /// <summary>
    /// Validates the specified settings and remaining arguments.
    /// </summary>
    /// <param name="context">The command context.</param>
    /// <param name="settings">The settings.</param>
    /// <returns>The validation result.</returns>
    public virtual ValidationResult Validate(CommandContext context, TCommandSettings settings)
    {
        return ValidationResult.Success();
    }

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="context">The command context.</param>
    /// <param name="settings">The settings.</param>
    /// <returns>An integer indicating whether or not the command executed successfully.</returns>
    public abstract Task<int> ExecuteAsync(CommandContext context, TCommandSettings settings);

    /// <inheritdoc/>
    ValidationResult ICommand.Validate(CommandContext context, ICommandSettings settings)
    {
        return Validate(context, (TCommandSettings)settings);
    }

    /// <inheritdoc/>
    Task<int> ICommand.Execute(CommandContext context, ICommandSettings settings)
    {
        Debug.Assert(settings is TCommandSettings, "Command settings is of unexpected type.");
        return ExecuteAsync(context, (TCommandSettings)settings);
    }

    /// <inheritdoc/>
    Task<int> ICommand<TCommandSettings>.Execute(CommandContext context, TCommandSettings settings)
    {
        return ExecuteAsync(context, settings);
    }
}