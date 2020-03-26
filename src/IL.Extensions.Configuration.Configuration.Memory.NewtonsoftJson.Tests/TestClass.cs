using System;

namespace IL.Extensions.Configuration.Configuration.Memory.NewtonsoftJson.Tests
{
    public class TestClass
    {
        public int IntValue { get; set; }

        public double DoubleValue { get; set; }

        public string StringValue { get; set; } = string.Empty;

#pragma warning disable CA1822
        public string? NullValue => null;
#pragma warning restore CA1822

        public DateTime DateTimeValue { get; set; }

        public DateTimeOffset DateTimeOffsetValue { get; set; }
    }
}
