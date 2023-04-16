namespace IL.Extensions.Options.ConfigurationExtensions.NewtonsoftJson.Tests;

public class TestClass
{
    public int IntValue { get; set; }

    public double DoubleValue { get; set; }

    public string StringValue { get; set; } = string.Empty;

    public string? NullValue { get; set; }

    public DateTime DateTimeValue { get; set; }

    public DateTimeOffset DateTimeOffsetValue { get; set; }

    public IReadOnlyList<string> StringList { get; set; } = Array.Empty<string>();

    public IReadOnlyList<int> IntList { get; set; } = Array.Empty<int>();

    public IReadOnlyList<string>? NullList { get; set; }
}
