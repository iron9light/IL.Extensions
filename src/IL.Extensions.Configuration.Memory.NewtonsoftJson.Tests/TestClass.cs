using System;

namespace IL.Extensions.Configuration.Memory.NewtonsoftJson.Tests
{
    public class TestClass
    {
        public int IntValue { get; set; }

        public double DoubleValue { get; set; }

        public string StringValue { get; set; } = string.Empty;

        public string? NullValue => null;

        public DateTime DateTimeValue { get; set; }

        public DateTimeOffset DateTimeOffsetValue { get; set; }
    }
}
