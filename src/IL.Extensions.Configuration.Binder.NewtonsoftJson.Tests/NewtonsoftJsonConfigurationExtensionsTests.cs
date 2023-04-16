using IL.Extensions.Configuration.Memory.NewtonsoftJson;

using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IL.Extensions.Configuration.Binder.NewtonsoftJson.Tests;

public class NewtonsoftJsonConfigurationExtensionsTests
{
    [Theory]
    [AutoData]
    public void ToJToken_can_convert_string_value(string s)
    {
        var token = GetConfigurationToken(s);
        token.Type.Should().Be(JTokenType.String);
        token.Value<string>().Should().Be(s);
    }

    [Theory]
    [AutoData]
    public void ToJToken_can_convert_string_list(IReadOnlyList<string> list)
    {
        var token = GetConfigurationToken(list);
        token.Type.Should().Be(JTokenType.Array);
        var array = (JArray)token;
        array.Count.Should().Be(list.Count);
        for (var i = 0; i < list.Count; ++i)
        {
            var element = array[i];
            element.Type.Should().Be(JTokenType.String);
            element.Value<string>().Should().Be(list[i]);
        }
    }

    [Theory]
    [AutoData]
    public void ToJToken_can_convert_string_list_with_null_value(IReadOnlyList<string?> list)
    {
        var token = GetConfigurationToken(list.Concat(new string?[] { null }));
        token.Type.Should().Be(JTokenType.Array);
        var array = (JArray)token;
        array.Count.Should().Be(list.Count + 1);
        for (var i = 0; i < list.Count; ++i)
        {
            var element = array[i];
            element.Type.Should().Be(JTokenType.String);
            element.Value<string>().Should().Be(list[i]);
        }

        var nullElement = array.Last();
        nullElement.Type.Should().Be(JTokenType.Null);
    }

    [Theory]
    [AutoData]
    public void ToJToken_can_convert_dictionary(IReadOnlyDictionary<string, string> dict)
    {
        var token = GetConfigurationToken(dict);
        token.Type.Should().Be(JTokenType.Object);
        var jObject = (JObject)token;
        jObject.Count.Should().Be(dict.Count);
        foreach (var keyValuePair in dict)
        {
            var valueToken = jObject[keyValuePair.Key];
            valueToken.Should().NotBeNull();
            valueToken!.Type.Should().Be(JTokenType.String);
            valueToken!.Value<string>().Should().Be(keyValuePair.Value);
        }
    }

    private static JToken GetConfigurationToken(object o, JsonSerializerSettings? settings = null, params string[] keyPrefix)
    {
        return new ConfigurationBuilder()
            .AddInMemoryObject(o, settings, keyPrefix)
            .Build()
            .ToJToken();
    }
}
