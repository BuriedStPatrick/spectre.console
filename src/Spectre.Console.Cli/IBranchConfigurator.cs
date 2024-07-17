namespace Spectre.Console.Cli;

/// <summary>
/// Represents a branch configurator.
/// </summary>
/// <typeparam name="TBranchConfigurator">
///     The <see cref="IBranchConfigurator{TBranchConfigurator}"/> implementation type.
/// </typeparam>
public interface IBranchConfigurator<out TBranchConfigurator>
    where TBranchConfigurator : IBranchConfigurator<TBranchConfigurator>
{
    /// <summary>
    /// Adds an alias (an alternative name) to the branch being configured.
    /// </summary>
    /// <param name="name">The alias to add to the branch being configured.</param>
    /// <returns>The same <typeparamref name="TBranchConfigurator"/> instance so that multiple calls can be chained.</returns>
    TBranchConfigurator WithAlias(string name);
}