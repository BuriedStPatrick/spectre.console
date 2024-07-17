namespace Spectre.Console.Cli;

/// <summary>
/// Represents a configurator.
/// </summary>
/// <typeparam name="TConfigurator">
///     The <typeparamref name="TConfigurator"/> implementation.
/// </typeparam>
/// <typeparam name="TCommandConfigurator">
///     The comamnd configurator implementation type.
/// </typeparam>
/// <typeparam name="TBranchConfigurator">
///     The <see cref="IBranchConfigurator{TBranchConfigurator}"/> imeplementation type.
/// </typeparam>
public interface IConfigurator<
    out TConfigurator,
    out TCommandConfigurator,
    out TBranchConfigurator
>
    where TConfigurator : IConfigurator<TConfigurator, TCommandConfigurator, TBranchConfigurator>
    where TCommandConfigurator : ICommandConfigurator<TCommandConfigurator>
    where TBranchConfigurator : IBranchConfigurator<TBranchConfigurator>
{
    /// <summary>
    ///     Sets the culture for the application.
    /// </summary>
    /// <param name="cultureInfo">The <see cref="CultureInfo"/> to set.</param>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    public TConfigurator WithCulture(CultureInfo cultureInfo);

    /// <summary>
    ///     Sets the version for the application to a specific value.
    /// </summary>
    /// <param name="version">The version to set.</param>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    public TConfigurator WithVersion(string version);

    /// <summary>
    ///     Sets the version based on the assembly.
    /// </summary>
    /// <param name="assembly">The assembly from which to read the version.</param>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    public TConfigurator WithVersionFromAssembly(Assembly assembly);

    /// <summary>
    ///     Hides the <c>DEFAULT</c> column that lists default values coming from the
    /// </summary>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    public TConfigurator WithHiddenDefaultOptionValues();

    /// <summary>
    ///     Configures the console.
    /// </summary>
    /// <param name="ansiConsole">The console instance to set.</param>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    /// <typeparam name="TAnsiConsole">The <see cref="IAnsiConsole"/> implementation type.</typeparam>
    public TConfigurator WithConsole<TAnsiConsole>(TAnsiConsole ansiConsole)
        where TAnsiConsole : IAnsiConsole;

    /// <summary>
    ///     Configures the console.
    /// </summary>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    public TConfigurator WithStrictParsing();

    /// <summary>
    ///     Tells the help writer whether to trim trailing period.
    /// </summary>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    public TConfigurator WithTrailingPeriodTrimming();

    /// <summary>
    ///     Configures the command line application to propagate all exceptions to the user.
    /// </summary>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    public TConfigurator WithExceptionPropagation();

    /// <summary>
    ///     Configures case-sensitivity.
    /// </summary>
    /// <param name="caseSensitivity">The case sensitivity to set.</param>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    public TConfigurator WithCaseSensitivity(CaseSensitivity caseSensitivity);

    /// <summary>
    ///     Tells the command line application to validate all examples before running the application.
    /// </summary>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    public TConfigurator WithExamplesValidation();

    /// <summary>
    ///     Sets the command interceptor to be used.
    /// </summary>
    /// <param name="interceptor">The <see cref="ICommandInterceptor"/> to set.</param>
    /// <typeparam name="TCommandInterceptor">The <see cref="ICommandInterceptor"/> implementation type.</typeparam>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    public TConfigurator WithInterceptor<TCommandInterceptor>(TCommandInterceptor interceptor)
        where TCommandInterceptor : ICommandInterceptor;

    /// <summary>
    ///     Sets the command interceptor to be used.
    /// </summary>
    /// <param name="interceptor">The <see cref="ICommandInterceptor"/> to set.</param>
    /// <typeparam name="TCommandInterceptor">The <see cref="ICommandInterceptor{TCommandSettings}"/> implementation type.</typeparam>
    /// <typeparam name="TCommandSettings">The <see cref="ICommandSettings"/> implementation type.</typeparam>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    public TConfigurator WithInterceptor<TCommandInterceptor, TCommandSettings>(TCommandInterceptor interceptor)
        where TCommandInterceptor : ICommandInterceptor<TCommandSettings>
        where TCommandSettings : ICommandSettings;

    /// <summary>
    ///     Sets the exception handler.
    /// </summary>
    /// <param name="exceptionHandler">The exception handler delegate.</param>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    public TConfigurator WithExceptionHandler(Action<Exception, ITypeResolver?> exceptionHandler);

    /// <summary>
    ///     Sets the exception handler.
    /// </summary>
    /// <param name="exceptionHandler">The exception handler delegate.</param>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    public TConfigurator WithExceptionHandler(Func<Exception, ITypeResolver?, int>? exceptionHandler);

    /// <summary>
    /// Sets the help provider for the application.
    /// </summary>
    /// <param name="helpProvider">The help provider to use.</param>
    /// <typeparam name="THelpProvider">The <see cref="IHelpProvider"/> implementation type.</typeparam>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    public TConfigurator WithHelpProvider<THelpProvider>(THelpProvider helpProvider)
        where THelpProvider : IHelpProvider;

    /// <summary>
    /// Sets the help provider for the application.
    /// </summary>
    /// <typeparam name="THelpProvider">The type of the help provider to instantiate at runtime and use.</typeparam>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    public TConfigurator WithHelpProvider<THelpProvider>()
        where THelpProvider : IHelpProvider;

    /// <summary>
    /// Adds an example of how to use the application.
    /// </summary>
    /// <param name="args">The example arguments.</param>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    TConfigurator WithExample(params string[] args);

    /// <summary>
    /// Adds a command.
    /// </summary>
    /// <typeparam name="TCommand">The command type.</typeparam>
    /// <param name="name">The name of the command.</param>
    /// <param name="configureCommand">The command configuration delegate.</param>
    /// <returns>A command configurator that can be used to configure the command further.</returns>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    TConfigurator AddCommand<TCommand>(
        string name,
        Action<TCommandConfigurator>? configureCommand = null)
        where TCommand : class, ICommand;

    /// <summary>
    /// Adds a command that executes a delegate.
    /// </summary>
    /// <typeparam name="TSettings">The command setting type.</typeparam>
    /// <param name="name">The name of the command.</param>
    /// <param name="func">The delegate to execute as part of command execution.</param>
    /// <param name="configureCommand">The command configuration delegate.</param>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    TConfigurator AddDelegate<TSettings, TCommandBuilder>(
        string name,
        Func<CommandContext, TSettings, int> func,
        Action<TCommandBuilder> configureCommand)
        where TSettings : ICommandSettings
        where TCommandBuilder : ICommandDefinitionBuilder<TSettings>;

    /// <summary>
    /// Adds a command that executes an async delegate.
    /// </summary>
    /// <typeparam name="TSettings">The command setting type.</typeparam>
    /// <param name="name">The name of the command.</param>
    /// <param name="func">The delegate to execute as part of command execution.</param>
    /// <param name="configureCommand">The command configuration delegate.</param>
    /// <returns>A command configurator that can be used to configure the command further.</returns>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    TConfigurator AddAsyncDelegate<TSettings>(
        string name,
        Func<CommandContext, TSettings, Task<int>> func,
        Action<TCommandConfigurator>? configureCommand = null)
        where TSettings : ICommandSettings;

    /// <summary>
    /// Adds a command branch.
    /// </summary>
    /// <typeparam name="TBranchSettings">The command setting type.</typeparam>
    /// <param name="name">The name of the command branch.</param>
    /// <param name="configureBranch">The command branch configurator.</param>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    TConfigurator AddBranch<TBranchSettings>(
        string name,
        Action<TBranchConfigurator> configureBranch)
        where TBranchSettings : ICommandSettings;

    /// <summary>
    /// Adds a command branch with settings.
    /// </summary>
    /// <typeparam name="TBranchSettings">The command setting type.</typeparam>
    /// <param name="name">The name of the command branch.</param>
    /// <param name="configureBranch">The command branch configurator.</param>
    /// <returns>The <typeparamref name="TConfigurator"/> instance.</returns>
    TConfigurator AddBranch<TBranchSettings>(
        string name,
        Action<TBranchSettings> configureBranch)
        where TBranchSettings : ICommandSettings;
}