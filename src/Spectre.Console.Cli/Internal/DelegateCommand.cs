namespace Spectre.Console.Cli;

internal sealed class DelegateCommand : ICommand
{
    private readonly Func<CommandContext, ICommandSettings, Task<int>> _func;

    public DelegateCommand(Func<CommandContext, ICommandSettings, Task<int>> func)
    {
        _func = func;
    }

    public Task<int> Execute(CommandContext context, ICommandSettings settings)
    {
        return _func(context, settings);
    }

    public ValidationResult Validate(CommandContext context, ICommandSettings settings)
    {
        return ValidationResult.Success();
    }
}