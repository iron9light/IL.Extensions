using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using AutoFixture.Xunit2;

using FluentAssertions;

using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

using Xunit;

namespace IL.Extensions.Configuration.Configuration.Memory.NewtonsoftJson.Tests
{
#pragma warning disable S2699 // Tests should include assertions
    public class NewtonsoftJsonMemoryConfigurationBuilderExtensionsTests
    {
        private static readonly string DefaultDateFormatString = new JsonSerializerSettings().DateFormatString;

        [Theory]
        [AutoData]
        public void AddInMemoryObject_can_add_string(string x)
        {
            AddInMemoryObject_can_add_primitive(x);
        }

        [Theory]
        [AutoData]

        public void AddInMemoryObject_can_add_number(int x)
        {
            AddInMemoryObject_can_add_primitive(x);
        }

        [Theory]
        [AutoData]
        public void AddInMemoryObject_can_add_double(double x)
        {
            AddInMemoryObject_can_add_primitive(x);
        }

        [Theory]
        [AutoData]
        public void AddInMemoryObject_can_add_DateTime(DateTime x)
        {
            AddInMemoryObject_can_add_primitive(x, DefaultDateFormatString);
        }

        [Theory]
        [AutoData]
        public void AddInMemoryObject_can_add_DateTime_with_format(DateTime x)
        {
            AddInMemoryObject_can_add_primitive(x, "G", new JsonSerializerSettings { DateFormatString = "G" });
        }

        [Theory]
        [AutoData]
        public void AddInMemoryObject_can_add_DateTime_with_settings(DateTime x)
        {
            AddInMemoryObject_can_add_primitive(
                x.ToUniversalTime(),
                DefaultDateFormatString,
                new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Utc }
                );
        }

        [Theory]
        [AutoData]
        public void AddInMemoryObject_can_add_DateTimeOffset(DateTimeOffset x)
        {
            AddInMemoryObject_can_add_primitive(x, DefaultDateFormatString);
        }

        [Theory]
        [AutoData]
        public void AddInMemoryObject_can_add_Array(IReadOnlyList<int> x)
        {
            var config = GetConfiguration(x);
            for (var i = 0; i < x.Count; ++i)
            {
                var value = config[i.ToString(CultureInfo.InvariantCulture)];
                value.Should().Be(x[i].ToString(CultureInfo.InvariantCulture));
            }
        }

        [Theory]
        [AutoData]
        public void AddInMemoryObject_can_add_object(TestClass x)
        {
            var config = GetConfiguration(x);
            Assert(config, x);
        }

        [Theory]
        [AutoData]
        public void AddInMemoryObject_can_add_object_array(IReadOnlyList<TestClass> array)
        {
            var configRoot = GetConfiguration(array);
            for (var i = 0; i < array.Count; ++i)
            {
                var x = array[i];
                var config = configRoot.GetSection(i.ToString(CultureInfo.InvariantCulture));
                Assert(config, x);
            }
        }

        [Theory]
        [AutoData]
        public void AddInMemoryObject_can_add_object_array_with_prefix(IReadOnlyList<string> keyPrefix, IReadOnlyList<TestClass> array)
        {
            var configRoot = GetConfiguration(array, keyPrefix: keyPrefix.ToArray());
            for (var i = 0; i < array.Count; ++i)
            {
                var x = array[i];
                var prefixString = ConfigurationPath.Combine(keyPrefix.Concat(new[] { i.ToString(CultureInfo.InvariantCulture) }));
                var config = configRoot.GetSection(prefixString);
                Assert(config, x);
            }
        }

        private static IConfiguration GetConfiguration(object o, JsonSerializerSettings? settings = null, params string[] keyPrefix)
        {
            return new ConfigurationBuilder()
                .AddInMemoryObject(o, settings, keyPrefix)
                .Build();
        }

        private static void AddInMemoryObject_can_add_primitive(
            IFormattable x,
            string? format = null,
            JsonSerializerSettings? settings = null
            )
        {
            var config = GetConfiguration(x, settings);
            var value = config[string.Empty];
            value.Should().Be(x.ToString(format, CultureInfo.InvariantCulture));
        }

        private static void AddInMemoryObject_can_add_primitive(
            object x,
            JsonSerializerSettings? settings = null
            )
        {
            var config = GetConfiguration(x, settings);
            var value = config[string.Empty];
            value.Should().Be(x.ToString());
        }

        private static void Assert(IConfiguration config, TestClass x)
        {
            config["IntValue"].Should().Be(x.IntValue.ToString(CultureInfo.InvariantCulture));
            config["DoubleValue"].Should().Be(x.DoubleValue.ToString(CultureInfo.InvariantCulture));
            config["StringValue"].Should().Be(x.StringValue);
            config["NullValue"].Should().BeNull();
            config["DateTimeValue"].Should().Be(x.DateTimeValue.ToString(DefaultDateFormatString, CultureInfo.InvariantCulture));
            config["DateTimeOffsetValue"].Should().Be(x.DateTimeOffsetValue.ToString(DefaultDateFormatString, CultureInfo.InvariantCulture));
        }
    }
#pragma warning restore S2699 // Tests should include assertions
}
