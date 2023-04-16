using System.Collections.Immutable;
using System.Globalization;

using Microsoft.Extensions.Configuration;

using Newtonsoft.Json.Linq;

namespace IL.Extensions.Configuration.Memory.NewtonsoftJson;

internal static class NewtonsoftJsonConfigurationJTokenParser
{
    public static IEnumerable<KeyValuePair<string, string?>> Parse(IEnumerable<string> keyPrefix, JToken token)
    {
        return VisitToken(keyPrefix.ToImmutableList(), token);
    }

    private static IEnumerable<KeyValuePair<string, string?>> VisitToken(IImmutableList<string> keys, JToken token)
    {
        switch (token.Type)
        {
            case JTokenType.Object:
                return VisitJObject(keys, token.Value<JObject>());

            case JTokenType.Array:
                return VisitArray(keys, token.Value<JArray>());

            case JTokenType.Null:
                return VisitNull(keys);

            case JTokenType.Integer:
            case JTokenType.Float:
            case JTokenType.String:
            case JTokenType.Boolean:
            case JTokenType.Bytes:
            case JTokenType.Raw:
                return VisitPrimitive(keys, token.Value<JValue>());

            case JTokenType.Undefined:
                return Array.Empty<KeyValuePair<string, string?>>();

            default:
                throw new InvalidOperationException($"Unsupported token type {token.Type}");
        }
    }

    private static IEnumerable<KeyValuePair<string, string?>> VisitJObject(IImmutableList<string> keys, JObject jObject)
    {
        return jObject.Properties()
            .SelectMany(property => VisitToken(keys.Add(property.Name), property.Value));
    }

    private static IEnumerable<KeyValuePair<string, string?>> VisitArray(IImmutableList<string> keys, JArray array)
    {
        return Enumerable.Range(0, array.Count)
            .SelectMany(i => VisitToken(keys.Add(i.ToString(CultureInfo.InvariantCulture)), array[i]));
    }

    private static IEnumerable<KeyValuePair<string, string?>> VisitPrimitive(IImmutableList<string> keys, JValue data)
    {
        var key = ConfigurationPath.Combine(keys);
        var value = data.ToString(CultureInfo.InvariantCulture);

        yield return new KeyValuePair<string, string?>(key, value);
    }

    private static IEnumerable<KeyValuePair<string, string?>> VisitNull(IImmutableList<string> keys)
    {
        var key = ConfigurationPath.Combine(keys);
        yield return new KeyValuePair<string, string?>(key, null);
    }
}
