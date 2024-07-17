namespace Spectre.Console.Cli;

internal sealed class UnsafeBranchConfigurator : IUnsafeBranchConfigurator<UnsafeBranchConfigurator>
{
    private readonly CommandDefinitionBuilder<,> _commandDefinitionBuilder;

    public UnsafeBranchConfigurator(CommandDefinitionBuilder<,> commandDefinitionBuilder)
    {
        _commandDefinitionBuilder = commandDefinitionBuilder;
    }

    public UnsafeBranchConfigurator WithDescription(string description)
    {
        _commandDefinitionBuilder.Description = description;

        return this;
    }

    public UnsafeBranchConfigurator WithExample(string[] args)
    {
        _commandDefinitionBuilder.Examples.Add(args);

        return this;
    }
}