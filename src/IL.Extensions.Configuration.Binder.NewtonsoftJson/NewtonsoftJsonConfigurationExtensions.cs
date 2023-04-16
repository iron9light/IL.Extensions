using System.Globalization;

using Microsoft.Extensions.Configuration;

using Newtonsoft.Json.Linq;

namespace IL.Extensions.Configuration.Binder.NewtonsoftJson;

/// <summary>
/// <see cref="IConfiguration"/> extension methods for converting to <see cref="JToken"/>.
/// </summary>
public static class NewtonsoftJsonConfigurationExtensions
{
    /// <summary>
    /// Converte <see cref="IConfiguration"/> to <see cref="JToken"/>.
    /// </summary>
    /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
    /// <returns>The <see cref="JToken"/>.</returns>
    public static JToken ToJToken(this IConfiguration configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        if (configuration is IConfigurationSection configurationSection)
        {
            return configurationSection.ToJToken();
        }

        if (!configuration.GetChildren().Any())
        {
            return new JObject();
        }

        var sections = configuration.GetChildren().ToList();

        if (sections.Count == 1 && string.IsNullOrEmpty(sections.First().Key))
        {
            var value = sections.First().Value;
            if (value == null)
            {
                return JValue.CreateNull();
            }
            else
            {
                return JValue.CreateString(value);
            }
        }
        else
        {
            return ToJObjectOrJArray(sections);
        }
    }

    private static JToken ToJToken(this IConfigurationSection section)
    {
        if (section == null)
        {
            throw new ArgumentNullException(nameof(section));
        }

        if (section.Value != null)
        {
            return JValue.CreateString(section.Value);
        }
        else if (!section.GetChildren().Any())
        {
            return JValue.CreateNull();
        }
        else
        {
            return ToJObjectOrJArray(section.GetChildren().ToList());
        }
    }

    private static JToken ToJObjectOrJArray(IReadOnlyList<IConfigurationSection> sections)
    {
        if (IsArray(sections))
        {
            var array = new JArray();
            foreach (var section in sections)
            {
                array.Add(section.ToJToken());
            }

            return array;
        }
        else
        {
            var jObject = new JObject();
            foreach (var section in sections)
            {
                jObject.Add(section.Key, section.ToJToken());
            }

            return jObject;
        }
    }

    private static bool IsArray(IReadOnlyList<IConfigurationSection> sections)
    {
        for (var i = 0; i < sections.Count; ++i)
        {
            if (sections[i].Key != i.ToString(CultureInfo.InvariantCulture))
            {
                return false;
            }
        }

        return true;
    }
}
