namespace Spectre.Console.Tests.Data;

public sealed class GenericCommand<TSettings> : Command<TSettings>
    where TSettings : ICommandSettings
{
    public override int Execute(CommandContext context, TSettings settings)
    {
        return 0;
    }
}
