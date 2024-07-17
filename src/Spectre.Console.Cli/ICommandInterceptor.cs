namespace Spectre.Console.Cli;

/// <summary>
/// Represents a command settings interceptor that
/// will intercept command settings before it's
/// passed to a command.
/// </summary>
public interface ICommandInterceptor
{
    /// <summary>
    /// Intercepts command information before it's passed to a command.
    /// </summary>
    /// <param name="context">The intercepted <see cref="CommandContext"/>.</param>
    /// <param name="settings">The intercepted <see cref="ICommandSettings"/>.</param>
    void Intercept(CommandContext context, ICommandSettings settings);

    /// <summary>
    /// Intercepts a command result before it's passed as the result.
    /// </summary>
    /// <param name="context">The intercepted <see cref="CommandContext"/>.</param>
    /// <param name="settings">The intercepted <see cref="ICommandSettings"/>.</param>
    /// <param name="result">The result from the command execution.</param>
    void InterceptResult(CommandContext context, ICommandSettings settings, ref int result);
}

/// <inheritdoc cref="ICommandInterceptor{TCommandSettings}"/>
public abstract class CommandInterceptor<TCommandSettings> : ICommandInterceptor<TCommandSettings>
    where TCommandSettings : ICommandSettings
{
    /// <inheritdoc />
    public abstract void Intercept(CommandContext context, TCommandSettings settings);

    /// <inheritdoc />
    public abstract void InterceptResult(CommandContext context, TCommandSettings settings, ref int result);

    /// <inheritdoc />
    public void Intercept(CommandContext context, ICommandSettings settings)
    {
        Intercept(context, (TCommandSettings)settings);
    }

    /// <inheritdoc />
    public void InterceptResult(CommandContext context, ICommandSettings settings, ref int result)
    {
        InterceptResult(context, (TCommandSettings)settings, ref result);
    }
}

/// <summary>
/// Represents a command settings interceptor that
/// will intercept command settings before it's
/// passed to a command.
/// </summary>
/// <typeparam name="TCommandSettings">The <see cref="ICommandSettings"/> implementation.</typeparam>
public interface ICommandInterceptor<in TCommandSettings> : ICommandInterceptor
    where TCommandSettings : ICommandSettings
{
    /// <summary>
    /// Intercepts command information before it's passed to a command.
    /// </summary>
    /// <param name="context">The intercepted <see cref="CommandContext"/>.</param>
    /// <param name="settings">The intercepted <see cref="ICommandSettings"/>.</param>
    void Intercept(CommandContext context, TCommandSettings settings)
#if NETSTANDARD2_0
    ;
#else
    {
    }
#endif

    /// <summary>
    /// Intercepts a command result before it's passed as the result.
    /// </summary>
    /// <param name="context">The intercepted <see cref="CommandContext"/>.</param>
    /// <param name="settings">The intercepted <see cref="ICommandSettings"/>.</param>
    /// <param name="result">The result from the command execution.</param>
    void InterceptResult(CommandContext context, TCommandSettings settings, ref int result)
#if NETSTANDARD2_0
    ;
#else
    {
    }
#endif
}