using System.Globalization;

using IL.Extensions.Configuration.Memory.NewtonsoftJson;

using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

namespace IL.Extensions.Configuration.Binder.NewtonsoftJson.Tests;

public class NewtonsoftJsonConfigurationBinderTests
{
    [Theory]
    [AutoData]
    public void ToObject_can_get_string(string x)
    {
        var config = GetConfiguration(x);
        var actual = config.ToObject<string>();
        actual.Should().Be(x);
    }

    [Theory]
    [AutoData]
    public void ToObject_can_get_string_with_prefix(string x, string prefix)
    {
        var config = GetConfiguration(x, null, prefix);
        var actual = config.ToObject<string>(prefix, defaultValue: string.Empty);
        actual.Should().Be(x);
    }

    [Theory]
    [AutoData]
    public void ToObject_can_get_int(int x)
    {
        var config = GetConfiguration(x);
        var actual = config.ToObject<int>();
        actual.Should().Be(x);
    }

    [Theory]
    [AutoData]
    public void ToObject_can_get_int_with_prefix(int x, string prefix)
    {
        var config = GetConfiguration(x, null, prefix);
        var actual = config.ToObject<int>(prefix);
        actual.Should().Be(x);
    }

    [Theory]
    [AutoData]
    public void ToObject_can_get_double(double x)
    {
        var config = GetConfiguration(x);
        var actual = config.ToObject<double>();
        actual.Should().Be(x);
    }

    [Theory]
    [AutoData]
    public void ToObject_can_get_double_with_prefix(double x, string prefix)
    {
        var config = GetConfiguration(x, null, prefix);
        var actual = config.ToObject<double>(prefix);
        actual.Should().Be(x);
    }

    [Theory]
    [AutoData]
    public void ToObject_can_get_DateTime(DateTime x)
    {
        x = x.ToUniversalTime();
        var config = GetConfiguration(x.ToString("o", CultureInfo.InvariantCulture));
        var actual = config.ToObject<DateTime>(settings: new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Utc });
        actual.Should().Be(x);
    }

    [Theory]
    [AutoData]
    public void ToObject_can_get_DateTime_with_prefix(DateTime x, string prefix)
    {
        x = x.ToUniversalTime();
        var config = GetConfiguration(x.ToString("o", CultureInfo.InvariantCulture), null, prefix);
        var actual = config.ToObject<DateTime>(prefix, settings: new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Utc });
        actual.Should().Be(x);
    }

    [Theory]
    [AutoData]
    public void ToObject_can_get_DateTimeOffset(DateTimeOffset x)
    {
        x = x.ToOffset(TimeSpan.FromHours(1.5));
        var config = GetConfiguration(x);
        var actual = config.ToObject<DateTimeOffset>();
        actual.Should().Be(x);
    }

    [Theory]
    [AutoData]
    public void ToObject_can_get_DateTimeOffset_with_prefix(DateTimeOffset x, string prefix)
    {
        x = x.ToOffset(TimeSpan.FromHours(1.5));
        var config = GetConfiguration(x, null, prefix);
        var actual = config.ToObject<DateTimeOffset>(prefix);
        actual.Should().Be(x);
    }

    [Theory]
    [AutoData]
    public void ToObject_can_get_list(IReadOnlyList<int> x)
    {
        var config = GetConfiguration(x);
        var actual = config.ToObject<IReadOnlyList<int>>();
        actual.Should().BeEquivalentTo(x);
    }

    [Theory]
    [AutoData]
    public void ToObject_can_get_list_with_prefix(IReadOnlyList<int> x, string prefix)
    {
        var config = GetConfiguration(x, null, prefix);
        var actual = config.ToObject<IReadOnlyList<int>>(prefix);
        actual.Should().BeEquivalentTo(x);
    }

    [Theory]
    [AutoData]
    public void ToObject_can_get_object(TestClass x)
    {
        x.NullValue = null;
        x.NullList = null;
        var config = GetConfiguration(x);
        var actual = config.ToObject<TestClass>();
        actual.Should().NotBeNull();
        actual!.Should().BeEquivalentTo(x);
    }

    [Theory]
    [AutoData]
    public void ToObject_can_get_object_with_prefix(TestClass x, string prefix)
    {
        x.NullValue = null;
        x.NullList = null;
        var config = GetConfiguration(x, null, prefix);
        var actual = config.ToObject<TestClass>(prefix);
        actual.Should().NotBeNull();
        actual!.Should().BeEquivalentTo(x);
    }

    [Fact]
    public void ToObject_with_empty_config_can_get_object()
    {
        var config = new ConfigurationBuilder().Build();
        var actual = config.ToObject<TestClass>();
        actual.Should().NotBeNull();
        actual!.Should().BeEquivalentTo(new TestClass());
    }

    [Theory]
    [AutoData]
    public void Populate_can_bind_object(TestClass x1, TestClass x2)
    {
        x1.NullValue = null;
        x1.NullList = null;
        var config = GetConfiguration(x1);
        config.Populate(x2);
        x2.Should().BeEquivalentTo(x1);
    }

    [Theory]
    [AutoData]
    public void Populate_can_bind_object_with_prefix(TestClass x1, TestClass x2, string prefix)
    {
        x1.NullValue = null;
        x1.NullList = null;
        var config = GetConfiguration(x1, null, prefix);
        config.Populate(prefix, x2);
        x2.Should().BeEquivalentTo(x1);
    }

    [Theory]
    [AutoData]
    public void Populate_with_empty_config_can_bind_object(TestClass x)
    {
        var xClone = JsonConvert.DeserializeObject<TestClass>(JsonConvert.SerializeObject(x));
        var config = new ConfigurationBuilder().Build();
        config.Populate(x);
        x.Should().BeEquivalentTo(xClone);
    }

    private static IConfiguration GetConfiguration(object o, JsonSerializerSettings? settings = null, params string[] keyPrefix)
    {
        return new ConfigurationBuilder()
            .AddInMemoryObject(o, settings, keyPrefix)
            .Build();
    }
}
