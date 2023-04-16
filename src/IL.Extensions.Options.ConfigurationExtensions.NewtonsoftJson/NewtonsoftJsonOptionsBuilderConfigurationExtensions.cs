using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace IL.Extensions.Options.ConfigurationExtensions.NewtonsoftJson;

/// <summary>
/// Extension methods for adding configuration related options services to the DI container via <see cref="OptionsBuilder{TOptions}"/>.
/// </summary>
public static class NewtonsoftJsonOptionsBuilderConfigurationExtensions
{
    /// <summary>
    /// Registers a configuration instance which <typeparamref name="TOptions"/> will bind against.
    /// </summary>
    /// <typeparam name="TOptions">The options type to be configured.</typeparam>
    /// <param name="optionsBuilder">The options builder to add the services to.</param>
    /// <param name="config">The configuration being bound.</param>
    /// <param name="settings">The <see cref="JsonSerializerSettings"/>.</param>
    /// <returns>The <see cref="OptionsBuilder{TOptions}"/> so that additional calls can be chained.</returns>
    public static OptionsBuilder<TOptions> BindAsJson<TOptions>(
        this OptionsBuilder<TOptions> optionsBuilder,
        IConfiguration config,
        JsonSerializerSettings? settings = null
        )
        where TOptions : class
    {
        if (optionsBuilder == null)
        {
            throw new ArgumentNullException(nameof(optionsBuilder));
        }

        optionsBuilder.Services.ConfigureAsJson<TOptions>(optionsBuilder.Name, config, settings: settings);
        return optionsBuilder;
    }
}
