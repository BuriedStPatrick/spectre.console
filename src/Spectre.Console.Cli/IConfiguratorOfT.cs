namespace Spectre.Console.Cli;

/// <summary>
///     Represents a configurator for specific settings.
/// </summary>
/// <typeparam name="TConfigurator">
///     The <see cref="IConfigurator{TConfigurator,TDefaultCommandSettings,TCommandConfigurator,TBranchConfigurator}"/> implementation.
/// </typeparam>
/// <typeparam name="TDefaultCommandSettings">
///     The command setting type.
/// </typeparam>
/// <typeparam name="TCommandConfigurator">
///     The <see cref="ICommandConfigurator{TCommandConfigurator}"/> implementation.
/// </typeparam>
/// <typeparam name="TBranchConfigurator">
///     The <see cref="IBranchConfigurator{TBranchConfigurator}"/> implementation.
/// </typeparam>
public interface IConfigurator<out TConfigurator, in TDefaultCommandSettings, out TCommandConfigurator, out TBranchConfigurator>
    where TConfigurator : IConfigurator<TConfigurator, TDefaultCommandSettings, TCommandConfigurator, TBranchConfigurator>
    where TDefaultCommandSettings : ICommandSettings
    where TCommandConfigurator : ICommandConfigurator<TCommandConfigurator>
    where TBranchConfigurator : IBranchConfigurator<TBranchConfigurator>
{
    /// <summary>
    /// Adds a default command.
    /// </summary>
    /// <remarks>
    /// This is the command that will run if the user doesn't specify one on the command line.
    /// It must be able to execute successfully by itself ie. without requiring any command line
    /// arguments, flags or option values.
    /// </remarks>
    /// <typeparam name="TDefaultCommand">The default command type.</typeparam>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    TConfigurator WithDefaultCommand<TDefaultCommand>()
        where TDefaultCommand : class, ICommand;

    /// <summary>
    /// Marks the branch as hidden.
    /// Hidden branches do not show up in help documentation or
    /// generated XML models.
    /// </summary>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    TConfigurator WithHiddenBranch();

    /// <summary>
    /// Adds a command.
    /// </summary>
    /// <typeparam name="TCommand">The command type.</typeparam>
    /// <typeparam name="TCommandSettings">The command settings.</typeparam>
    /// <param name="name">The name of the command.</param>
    /// <param name="configureCommand">The <typeparamref name="TCommandConfigurator"/> delegate.</param>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    TConfigurator AddCommand<TCommand, TCommandSettings>(
        string name,
        Action<TCommandConfigurator>? configureCommand = null)
        where TCommand : ICommand<TCommandSettings>
        where TCommandSettings : ICommandSettings;

    /// <summary>
    /// Adds a command that executes a delegate.
    /// </summary>
    /// <typeparam name="TDerivedSettings">The derived command setting type.</typeparam>
    /// <param name="name">The name of the command.</param>
    /// <param name="func">The delegate to execute as part of command execution.</param>
    /// <param name="configureCommand">The <typeparamref name="TCommandConfigurator"/> delegate.</param>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    TConfigurator AddDelegate<TDerivedSettings>(
        string name,
        Func<CommandContext, TDerivedSettings, int> func,
        Action<TCommandConfigurator>? configureCommand = null)
        where TDerivedSettings : TDefaultCommandSettings;

    /// <summary>
    /// Adds a command that executes an async delegate.
    /// </summary>
    /// <typeparam name="TDerivedSettings">The derived command setting type.</typeparam>
    /// <param name="name">The name of the command.</param>
    /// <param name="func">The delegate to execute as part of command execution.</param>
    /// <param name="configureCommand">The <typeparamref name="TCommandConfigurator"/> delegate.</param>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    TConfigurator AddAsyncDelegate<TDerivedSettings>(
        string name,
        Func<CommandContext, TDerivedSettings, Task<int>> func,
        Action<TCommandConfigurator>? configureCommand = null)
        where TDerivedSettings : TDefaultCommandSettings;

    /// <summary>
    /// Adds a command branch.
    /// </summary>
    /// <typeparam name="TDerivedSettings">The derived command setting type.</typeparam>
    /// <param name="name">The name of the command branch.</param>
    /// <param name="configureBranch">The command branch configuration.</param>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    TConfigurator AddBranch<TDerivedSettings>(
        string name,
        Action<TBranchConfigurator> configureBranch)
        where TDerivedSettings : ICommandSettings;
}