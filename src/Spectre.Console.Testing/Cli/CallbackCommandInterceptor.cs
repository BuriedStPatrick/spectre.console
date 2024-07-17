namespace Spectre.Console.Testing;

/// <summary>
/// A <see cref="ICommandInterceptor{CommandSettings}"/> that triggers a callback when invoked.
/// </summary>
public sealed class CallbackCommandInterceptor : CommandInterceptor<CommandSettings>
{
    private readonly Action<CommandContext, CommandSettings> _callback;

    /// <summary>
    /// Initializes a new instance of the <see cref="CallbackCommandInterceptor"/> class.
    /// </summary>
    /// <param name="callback">The callback to call when the interceptor is invoked.</param>
    public CallbackCommandInterceptor(Action<CommandContext, CommandSettings> callback)
    {
        _callback = callback ?? throw new ArgumentNullException(nameof(callback));
    }

    /// <inheritdoc/>
    public override void Intercept(CommandContext context, CommandSettings settings)
    {
        _callback(context, settings);
    }

    /// <inheritdoc/>
    public override void InterceptResult(CommandContext context, CommandSettings settings, ref int result)
    {
    }

#if NETSTANDARD2_0
    /// <inheritdoc/>
    public void InterceptResult(CommandContext context, CommandSettings settings, ref int result)
    {
    }
#endif
}