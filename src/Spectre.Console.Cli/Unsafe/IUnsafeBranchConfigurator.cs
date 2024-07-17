namespace Spectre.Console.Cli.Unsafe;

/// <summary>
/// Represents an unsafe configurator for a branch.
/// </summary>
/// <typeparam name="TUnsafeBranchConfigurator">
///     The <see cref="IUnsafeBranchConfigurator{TUnsafeBranchConfigurator}"/> implementation type.
/// </typeparam>
public interface IUnsafeBranchConfigurator<out TUnsafeBranchConfigurator>
    where TUnsafeBranchConfigurator : IUnsafeBranchConfigurator<TUnsafeBranchConfigurator>
{
    /// <summary>
    /// Sets the description of the branch.
    /// </summary>
    /// <param name="description">The description of the branch.</param>
    /// <returns>The current configurator instance.</returns>
    TUnsafeBranchConfigurator WithDescription(string description);

    /// <summary>
    /// Adds an example of how to use this branch.
    /// </summary>
    /// <param name="args">The example arguments.</param>
    /// <returns>The current configurator instance.</returns>
    TUnsafeBranchConfigurator WithExample(string[] args);
}