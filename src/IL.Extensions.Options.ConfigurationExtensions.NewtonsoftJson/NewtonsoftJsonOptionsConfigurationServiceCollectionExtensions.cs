using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace IL.Extensions.Options.ConfigurationExtensions.NewtonsoftJson;

/// <summary>
/// Extension methods for adding configuration related options services to the DI container.
/// </summary>
public static class NewtonsoftJsonOptionsConfigurationServiceCollectionExtensions
{
    /// <summary>
    /// Registers a configuration instance which TOptions will bind against.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being configured.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="config">The configuration being bound.</param>
    /// <param name="settings">The <see cref="JsonSerializerSettings"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection ConfigureAsJson<TOptions>(
        this IServiceCollection services,
        IConfiguration config,
        JsonSerializerSettings? settings = null
        )
        where TOptions : class
        => services.ConfigureAsJson<TOptions>(Microsoft.Extensions.Options.Options.DefaultName, config, settings: settings);

    /// <summary>
    /// Registers a configuration instance which TOptions will bind against.
    /// </summary>
    /// <typeparam name="TOptions">The type of options being configured.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="name">The name of the options instance.</param>
    /// <param name="config">The configuration being bound.</param>
    /// <param name="settings">The <see cref="JsonSerializerSettings"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection ConfigureAsJson<TOptions>(
        this IServiceCollection services,
        string name,
        IConfiguration config,
        JsonSerializerSettings? settings = null
        )
        where TOptions : class
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (config == null)
        {
            throw new ArgumentNullException(nameof(config));
        }

        services.AddOptions();
        services.AddSingleton<IOptionsChangeTokenSource<TOptions>>(
            new ConfigurationChangeTokenSource<TOptions>(name, config)
            );
        return services.AddSingleton<IConfigureOptions<TOptions>>(
            new NewtonsoftJsonNamedConfigureFromConfigurationOptions<TOptions>(name, config, settings: settings)
            );
    }
}
