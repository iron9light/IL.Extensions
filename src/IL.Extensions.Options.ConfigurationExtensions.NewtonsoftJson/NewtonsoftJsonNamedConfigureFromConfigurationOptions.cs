using IL.Extensions.Configuration.Binder.NewtonsoftJson;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;

namespace IL.Extensions.Options.ConfigurationExtensions.NewtonsoftJson;

/// <summary>
/// Configures an option instance by using <see cref="NewtonsoftJsonConfigurationBinder.Populate(IConfiguration, object, JsonSerializerSettings?)"/> against an <see cref="IConfiguration"/>.
/// </summary>
/// <typeparam name="TOptions">The type of options to bind.</typeparam>
public class NewtonsoftJsonNamedConfigureFromConfigurationOptions<TOptions>
    : ConfigureNamedOptions<TOptions>
    where TOptions : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NewtonsoftJsonNamedConfigureFromConfigurationOptions{TOptions}"/> class.
    /// Constructor that takes the <see cref="IConfiguration"/> instance to bind against.
    /// </summary>
    /// <param name="name">The name of the options instance.</param>
    /// <param name="config">The <see cref="IConfiguration"/> instance.</param>
    /// <param name="settings">The <see cref="JsonSerializerSettings"/>.</param>
    public NewtonsoftJsonNamedConfigureFromConfigurationOptions(
        string? name,
        IConfiguration config,
        JsonSerializerSettings? settings = null
        )
        : base(name, options => config.Populate(options, settings: settings))
    {
        if (config == null)
        {
            throw new ArgumentNullException(nameof(config));
        }
    }
}
