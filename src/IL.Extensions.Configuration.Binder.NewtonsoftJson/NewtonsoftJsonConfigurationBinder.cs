using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

namespace IL.Extensions.Configuration.Binder.NewtonsoftJson
{
    /// <summary>
    /// Static helper class that allows binding strongly typed objects to configuration values using the <see cref="JsonSerializer"/>.
    /// </summary>
    public static class NewtonsoftJsonConfigurationBinder
    {
        /// <summary>
        /// Creates an instance of the specified .NET type from the <see cref="IConfiguration"/> using the <see cref="JsonSerializer"/>.
        /// </summary>
        /// <typeparam name="T">The object type that the <see cref="IConfiguration"/> will be deserialized to.</typeparam>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="settings">The <see cref="JsonSerializerSettings"/>.</param>
        /// <returns>The new object created from the <see cref="IConfiguration"/>.</returns>
        [return: MaybeNull]
        public static T ToObject<T>(this IConfiguration configuration, JsonSerializerSettings? settings = null)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var token = configuration.ToJToken();

            if (settings == null)
            {
                return token.ToObject<T>();
            }
            else
            {
                var serializer = JsonSerializer.CreateDefault(settings);
                return token.ToObject<T>(serializer);
            }
        }

        /// <summary>
        /// Creates an instance of the specified .NET type from the <see cref="IConfiguration"/> using the <see cref="JsonSerializer"/>.
        /// </summary>
        /// <typeparam name="T">The object type that the <see cref="IConfiguration"/> will be deserialized to.</typeparam>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="key">The key of the configuration section.</param>
        /// <param name="defaultValue">The default value when deserialized value is null.</param>
        /// <param name="settings">The <see cref="JsonSerializerSettings"/>.</param>
        /// <returns>The new object created from the <see cref="IConfiguration"/>.</returns>
        [return: MaybeNull]
        [return: NotNullIfNotNull("defaultValue")]
        public static T ToObject<T>(
            this IConfiguration configuration,
            string key,
            T defaultValue = default,
            JsonSerializerSettings? settings = null
            )
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return configuration.GetSection(key).ToObject<T>(settings: settings) ?? defaultValue;
        }

        /// <summary>
        /// Populates the <see cref="IConfiguration"/> onto the target object using the <see cref="JsonSerializer"/>.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="key">The key of the configuration section.</param>
        /// <param name="instance">The target object to populate values onto.</param>
        /// <param name="settings">The <see cref="JsonSerializerSettings"/>.</param>
        public static void Populate(this IConfiguration configuration, string key, object instance, JsonSerializerSettings? settings = null)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            configuration.GetSection(key).Populate(instance, settings: settings);
        }

        /// <summary>
        /// Populates the <see cref="IConfiguration"/> onto the target object using the <see cref="JsonSerializer"/>.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="instance">The target object to populate values onto.</param>
        /// <param name="settings">The <see cref="JsonSerializerSettings"/>.</param>
        public static void Populate(this IConfiguration configuration, object instance, JsonSerializerSettings? settings = null)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var token = configuration.ToJToken();
            var serializer = JsonSerializer.CreateDefault(settings);

            using (var reader = token.CreateReader())
            {
                serializer.Populate(reader, instance);
            }
        }
    }
}
