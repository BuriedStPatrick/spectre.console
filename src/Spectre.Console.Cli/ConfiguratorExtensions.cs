namespace Spectre.Console.Cli;

/// <summary>
/// Contains extensions for <see cref="IConfigurator{TCommandConfigurator,TDefaultCommandSettings,TConfigurator}"/>.
/// </summary>
public static class ConfiguratorExtensions
{
    /// <summary>
    /// Sets the command interceptor to be used.
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <param name="interceptor">A <see cref="ICommandInterceptor"/>.</param>
    /// <returns>A configurator that can be used to configure the application further.</returns>
    /// <typeparam name="TConfigurator">
    ///     The <see cref="IConfigurator{TCommandConfigurator,TDefaultCommandSettings,TConfigurator}"/> implementation type.
    /// </typeparam>
    /// <typeparam name="TCommandConfigurator">
    ///     The <see cref="ICommandConfigurator{TCommandConfigurator}"/> implementation type.
    /// </typeparam>
    // public static TConfigurator WithInterceptor<TConfigurator, TCommandConfigurator>(this TConfigurator configurator, ICommandInterceptor interceptor)
    //     where TConfigurator : IConfigurator<TConfigurator, TCommandConfigurator>
    //     where TCommandConfigurator : ICommandConfigurator<TCommandConfigurator>
    // {
    //     configurator.Settings.Registrar.RegisterInstance(interceptor);
    //
    //     return configurator;
    // }

    /// <summary>
    /// Adds a command branch.
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <param name="name">The name of the command branch.</param>
    /// <param name="action">The branch configuration delegate.</param>
    /// <returns>A branch configurator that can be used to configure the branch further.</returns>
    /// <typeparam name="TConfigurator">
    ///     The <see cref="IConfigurator{TCommandConfigurator,TDefaultCommandSettings,TConfigurator}"/> implementation type.
    /// </typeparam>
    /// <typeparam name="TCommandConfigurator">
    ///     The <see cref="ICommandConfigurator{TCommandConfigurator}"/> implementation type.
    /// </typeparam>
    /// <typeparam name="TBranchConfigurator">
    ///     The branch configurator implementation type.
    /// </typeparam>
    /// <typeparam name="TBranchSettings">
    ///     The branch settings implementation type.
    /// </typeparam>
    // public static TConfigurator AddBranch<TConfigurator, TCommandConfigurator, TBranchConfigurator, TBranchSettings>(
    //     this TConfigurator configurator,
    //     string name,
    //     Action<TBranchConfigurator> action)
    //     where TConfigurator : IConfigurator<TConfigurator, TCommandConfigurator>
    //     where TCommandConfigurator : ICommandConfigurator<TCommandConfigurator>
    //     where TBranchConfigurator :
    //         IConfigurator<TBranchConfigurator, TBranchSettings, TCommandConfigurator>,
    //         IBranchConfigurator<TBranchConfigurator>
    //     where TBranchSettings : ICommandSettings
    // {
    //     return configurator.AddBranch<TBranchSettings, TBranchConfigurator>(name, action);
    // }

    /// <summary>
    /// Adds a command branch.
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <param name="name">The name of the command branch.</param>
    /// <param name="action">The command branch configuration.</param>
    /// <returns>A branch configurator that can be used to configure the branch further.</returns>
    /// <typeparam name="TConfigurator">
    ///     The <see cref="IConfigurator{TCommandConfigurator,TDefaultCommandSettings,TConfigurator}"/> implementation type.
    /// </typeparam>
    /// <typeparam name="TCommandConfigurator">
    ///     The <see cref="ICommandConfigurator{TCommandConfigurator}"/> implementation type.
    /// </typeparam>
    /// <typeparam name="TBranchConfigurator">
    ///     The branch configurator implementation type.
    /// </typeparam>
    /// <typeparam name="TBranchSettings">
    ///     The branch settings implementation type.
    /// </typeparam>
    // public static TConfigurator AddBranch<TConfigurator, TCommandConfigurator, TBranchConfigurator, TBranchSettings>(
    //     this TConfigurator configurator,
    //     string name,
    //     Action<TBranchSettings> action)
    //     where TConfigurator : IConfigurator<TConfigurator, TCommandConfigurator>
    //     where TCommandConfigurator : ICommandConfigurator<TCommandConfigurator>
    //     where TBranchConfigurator :
    //         IConfigurator<TBranchConfigurator, TBranchSettings, TCommandConfigurator>,
    //         IBranchConfigurator<TBranchConfigurator>
    //     where TBranchSettings : ICommandSettings =>
    //     configurator.AddBranch<TBranchSettings, TBranchConfigurator>(
    //         name,
    //         action);

    /// <summary>
    /// Adds a command without settings that executes a delegate.
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <param name="name">The name of the command.</param>
    /// <param name="func">The delegate to execute as part of command execution.</param>
    /// <param name="configureCommand">
    ///     <see cref="ICommandConfigurator{TCommandConfigurator}"/> delegate.
    ///     Use to further configure command.
    /// </param>
    /// <returns>The <see cref="IConfigurator{TConfigurator,TDefaultCommandSettings,TCommandConfigurator}"/> instance.</returns>
    /// <typeparam name="TConfigurator">
    ///     The <see cref="IConfigurator{TCommandConfigurator,TDefaultCommandSettings,TConfigurator}"/> implementation type.
    /// </typeparam>
    /// <typeparam name="TCommandConfigurator">
    ///     The <see cref="ICommandConfigurator{TCommandConfigurator}"/> implementation type.
    /// </typeparam>
    // public static TConfigurator AddDelegate<TConfigurator, TCommandConfigurator>(
    //     this TConfigurator configurator,
    //     string name,
    //     Func<CommandContext, int> func,
    //     Action<TCommandConfigurator>? configureCommand = null)
    //     where TConfigurator : IConfigurator<TConfigurator, TCommandConfigurator>
    //     where TCommandConfigurator : ICommandConfigurator<TCommandConfigurator> =>
    //     configurator.AddDelegate<EmptyCommandSettings>(
    //         name,
    //         (c, _) => func(c),
    //         configureCommand);

    /// <summary>
    /// Adds a command without settings that executes an async delegate.
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <param name="name">The name of the command.</param>
    /// <param name="func">The delegate to execute as part of command execution.</param>
    /// <param name="configureCommand">
    ///     <see cref="ICommandConfigurator{TCommandConfigurator}"/> delegate.
    ///     Use to further configure command.
    /// </param>
    /// <returns>A command configurator that can be used to configure the command further.</returns>
    /// <typeparam name="TConfigurator">
    ///     The <see cref="IConfigurator{TCommandConfigurator,TDefaultCommandSettings,TConfigurator}"/> implementation type.
    /// </typeparam>
    /// <typeparam name="TCommandConfigurator">
    ///     The <see cref="ICommandConfigurator{TCommandConfigurator}"/> implementation type.
    /// </typeparam>
    // public static TConfigurator AddAsyncDelegate<TConfigurator, TCommandConfigurator>(
    //     this TConfigurator configurator,
    //     string name,
    //     Func<CommandContext, Task<int>> func,
    //     Action<TCommandConfigurator>? configureCommand = null)
    //     where TConfigurator : IConfigurator<TConfigurator, TCommandConfigurator>
    //     where TCommandConfigurator : ICommandConfigurator<TCommandConfigurator> =>
    //     configurator.AddAsyncDelegate<EmptyCommandSettings>(
    //         name,
    //         (c, _) => func(c),
    //         configureCommand);

    /// <summary>
    /// Adds a command without settings that executes a delegate.
    /// </summary>
    /// <typeparam name="TSettings">The command setting type.</typeparam>
    /// <param name="configurator">The configurator.</param>
    /// <param name="name">The name of the command.</param>
    /// <param name="func">The delegate to execute as part of command execution.</param>
    /// <returns>A command configurator that can be used to configure the command further.</returns>
    // public static TConfigurator AddDelegate<TConfigurator, TConfiguratorSettings, TCommandConfigurator>(
    //     this TConfigurator? configurator,
    //     string name,
    //     Func<CommandContext, int> func)
    //     where TConfigurator : IConfigurator<TConfigurator, TConfiguratorSettings, TCommandConfigurator>
    //     where TConfiguratorSettings : ICommandSettings
    //     where TCommandConfigurator : ICommandConfigurator<TCommandConfigurator>
    // {
    //     if (typeof(TConfiguratorSettings).IsAbstract)
    //     {
    //         return AddDelegate<TConfigurator>(
    //             configurator,
    //             name,
    //             func);
    //
    //         // AddDelegate(configurator as IConfigurator<EmptyCommandSettings>, name, func);
    //     }
    //
    //     return configurator.AddDelegate<TSettings>(name, (c, _) => func(c));
    // }

    /// <summary>
    /// Adds a command without settings that executes an async delegate.
    /// </summary>
    /// <typeparam name="TSettings">The command setting type.</typeparam>
    /// <param name="configurator">The configurator.</param>
    /// <param name="name">The name of the command.</param>
    /// <param name="func">The delegate to execute as part of command execution.</param>
    /// <returns>A command configurator that can be used to configure the command further.</returns>
    // public static ICommandConfigurator AddAsyncDelegate<TSettings>(
    //     this IConfigurator<TSettings> configurator,
    //     string name,
    //     Func<CommandContext, Task<int>> func)
    //     where TSettings : ICommandSettings
    // {
    //     if (configurator == null)
    //     {
    //         throw new ArgumentNullException(nameof(configurator));
    //     }
    //
    //     return configurator.AddAsyncDelegate<TSettings>(name, (c, _) => func(c));
    // }

    /// <summary>
    /// Sets the ExceptionsHandler.
    /// <para>Setting <see cref="ICommandAppSettings.ExceptionHandler"/> this way will use the
    /// default exit code of -1.</para>
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <param name="exceptionHandler">The Action that handles the exception.</param>
    /// <returns>A configurator that can be used to configure the application further.</returns>
    // public static IConfigurator SetExceptionHandler(this IConfigurator configurator, Action<Exception, ITypeResolver?> exceptionHandler)
    // {
    //     return configurator.SetExceptionHandler((ex, resolver) =>
    //     {
    //         exceptionHandler(ex, resolver);
    //         return -1;
    //     });
    // }

    /// <summary>
    /// Sets the ExceptionsHandler.
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <param name="exceptionHandler">The Action that handles the exception.</param>
    /// <returns>A configurator that can be used to configure the application further.</returns>
    // public static IConfigurator SetExceptionHandler(this IConfigurator configurator, Func<Exception, ITypeResolver?, int>? exceptionHandler)
    // {
    //     if (configurator == null)
    //     {
    //         throw new ArgumentNullException(nameof(configurator));
    //     }
    //
    //     configurator.Settings.ExceptionHandler = exceptionHandler;
    //     return configurator;
    // }
}