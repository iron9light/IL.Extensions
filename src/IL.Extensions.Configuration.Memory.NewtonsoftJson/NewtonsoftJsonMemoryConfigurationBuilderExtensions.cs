using System;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IL.Extensions.Configuration.Memory.NewtonsoftJson
{
    /// <summary>
    /// IConfigurationBuilder extension methods for the MemoryConfigurationProvider with JSON string or JSON serializable object.
    /// </summary>
    public static class NewtonsoftJsonMemoryConfigurationBuilderExtensions
    {
        private static readonly JsonSerializerSettings ParseSettings = new JsonSerializerSettings { DateParseHandling = DateParseHandling.None };

        /// <summary>
        /// Adds the memory configuration provider to <paramref name="configurationBuilder"/>.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="o">The JSON serializable object ot add to memory configuration provider.</param>
        /// <param name="settings">The settings for JSON serialize.</param>
        /// <param name="keyPrefix">The prefix of configuration key.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddInMemoryObject(
            this IConfigurationBuilder configurationBuilder,
            object o,
            JsonSerializerSettings? settings = null,
            params string[] keyPrefix
            )
            => configurationBuilder.AddInMemoryObject(o, settings, (IEnumerable<string>)keyPrefix);

        /// <summary>
        /// Adds the memory configuration provider to <paramref name="configurationBuilder"/>.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="o">The JSON serializable object ot add to memory configuration provider.</param>
        /// <param name="settings">The settings for JSON serialize.</param>
        /// <param name="keyPrefix">The prefix of configuration key.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddInMemoryObject(
            this IConfigurationBuilder configurationBuilder,
            object o,
            JsonSerializerSettings? settings,
            IEnumerable<string> keyPrefix
            )
        {
            if (o == null)
            {
                throw new ArgumentNullException(nameof(o));
            }

            var json = JsonConvert.SerializeObject(o, Formatting.None, settings);
            return configurationBuilder.AddInMemoryJson(json, keyPrefix);
        }

        /// <summary>
        /// Adds the memory configuration provider to <paramref name="configurationBuilder"/>.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="json">The json to add to memory configuration provider.</param>
        /// <param name="keyPrefix">The prefix of configuration key.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddInMemoryJson(
            this IConfigurationBuilder configurationBuilder,
            string json,
            params string[] keyPrefix
            )
            => configurationBuilder.AddInMemoryJson(json, (IEnumerable<string>)keyPrefix);

        /// <summary>
        /// Adds the memory configuration provider to <paramref name="configurationBuilder"/>.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="json">The json to add to memory configuration provider.</param>
        /// <param name="keyPrefix">The prefix of configuration key.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddInMemoryJson(
            this IConfigurationBuilder configurationBuilder,
            string json,
            IEnumerable<string> keyPrefix
            )
        {
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            var token = JsonConvert.DeserializeObject<JToken>(json, ParseSettings)!;

            var data = NewtonsoftJsonConfigurationJTokenParser.Parse(keyPrefix, token);
            return configurationBuilder.AddInMemoryCollection(data);
        }
    }
}
