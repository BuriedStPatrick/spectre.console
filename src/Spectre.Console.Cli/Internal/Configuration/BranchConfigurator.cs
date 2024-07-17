namespace Spectre.Console.Cli;

internal sealed class BranchConfigurator : IBranchConfigurator<BranchConfigurator>
{
    public ICommandDefinitionBuilder CommandDefinitionBuilder { get; }

    public BranchConfigurator(ICommandDefinitionBuilder commandBuilder)
    {
        CommandDefinitionBuilder = commandBuilder;
    }

    public BranchConfigurator WithAlias(string alias)
    {
        CommandDefinitionBuilder.Aliases.Add(alias);

        return this;
    }
}