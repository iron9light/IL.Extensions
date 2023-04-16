using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace IL.Extensions.Options.ConfigurationExtensions.NewtonsoftJson;

/// <summary>
/// Creates <see cref="IChangeToken"/>s so that <see cref="IOptionsMonitor{TOptions}"/> gets
/// notified when <see cref="IConfiguration"/> changes.
/// </summary>
/// <typeparam name="TOptions">The type of options.</typeparam>
public class ConfigurationChangeTokenSource<TOptions>
    : IOptionsChangeTokenSource<TOptions>
{
    private readonly IConfiguration _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationChangeTokenSource{TOptions}"/> class.
    /// Constructor taking the <see cref="IConfiguration"/> instance to watch.
    /// </summary>
    /// <param name="config">The configuration instance.</param>
    public ConfigurationChangeTokenSource(IConfiguration config)
        : this(Microsoft.Extensions.Options.Options.DefaultName, config)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationChangeTokenSource{TOptions}"/> class.
    /// Constructor taking the <see cref="IConfiguration"/> instance to watch.
    /// </summary>
    /// <param name="name">The name of the options instance being watched.</param>
    /// <param name="config">The configuration instance.</param>
    public ConfigurationChangeTokenSource(string? name, IConfiguration config)
    {
        if (config == null)
        {
            throw new ArgumentNullException(nameof(config));
        }

        _config = config;
        Name = name ?? Microsoft.Extensions.Options.Options.DefaultName;
    }

    /// <summary>
    /// Gets the name of the option instance being changed.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Returns the reloadToken from the <see cref="IConfiguration"/>.
    /// </summary>
    /// <returns>The <see cref="IChangeToken"/>.</returns>
    public IChangeToken GetChangeToken()
    {
        return _config.GetReloadToken();
    }
}
