namespace Spectre.Console.Cli;

/// <summary>
/// Represents settings for a command.
/// </summary>
public interface ICommandSettings
{
    /// <summary>
    /// Performs validation of the settings.
    /// </summary>
    /// <returns>The validation result.</returns>
    ValidationResult Validate();
}

/// <summary>
/// Abstract base class for command settings.
/// </summary>
public abstract class CommandSettings : ICommandSettings
{
    /// <inheritdoc />
    public virtual ValidationResult Validate()
    {
        return ValidationResult.Success();
    }
}