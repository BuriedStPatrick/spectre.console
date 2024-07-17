namespace Spectre.Console.Cli.Unsafe;

/// <summary>
/// Represents an unsafe configurator.
/// </summary>
/// <typeparam name="TUnsafeConfigurator">
///     The <see cref="IUnsafeConfigurator{TUnsafeConfigurator,TUnsafeBranchConfigurator,TCommandConfigurator}"/> implementation type.
/// </typeparam>
/// <typeparam name="TUnsafeBranchConfigurator">
///     The <see cref="IUnsafeBranchConfigurator{TUnsafeBranchConfigurator}"/> implementation type.
/// </typeparam>
public interface IUnsafeConfigurator<out TUnsafeConfigurator, out TUnsafeBranchConfigurator, out TCommandConfigurator>
    where TUnsafeConfigurator : IUnsafeConfigurator<TUnsafeConfigurator, TUnsafeBranchConfigurator, TCommandConfigurator>
    where TUnsafeBranchConfigurator : IUnsafeBranchConfigurator<TUnsafeBranchConfigurator>
{
    /// <summary>
    /// Adds a command via reflection.
    /// </summary>
    /// <param name="name">The name of the command.</param>
    /// <param name="command">The command type.</param>
    /// <param name="configureCommand">Configure command delegate.</param>
    /// <returns>A command configurator that can be used to configure the command further.</returns>
    TUnsafeConfigurator AddCommand(
        string name,
        Type command,
        Action<TCommandConfigurator>? configureCommand = null);

    /// <summary>
    /// Adds a command branch.
    /// </summary>
    /// <param name="name">The name of the command branch.</param>
    /// <param name="settings">The command setting type.</param>
    /// <param name="configureBranch">The command branch configurator.</param>
    /// <returns>A branch configurator that can be used to configure the branch further.</returns>
    TUnsafeConfigurator AddBranch(
        string name,
        Type settings,
        Action<TUnsafeBranchConfigurator> configureBranch);
}