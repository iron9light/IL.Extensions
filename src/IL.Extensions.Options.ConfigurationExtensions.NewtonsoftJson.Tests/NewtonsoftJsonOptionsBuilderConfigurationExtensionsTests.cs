using IL.Extensions.Configuration.Memory.NewtonsoftJson;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace IL.Extensions.Options.ConfigurationExtensions.NewtonsoftJson.Tests;

public class NewtonsoftJsonOptionsBuilderConfigurationExtensionsTests
{
    [Theory]
    [AutoData]
    public async Task BindAsJson_can_bind_options(TestClass testClass)
    {
        testClass.NullValue = null;
        testClass.NullList = null;
        var hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(builder => builder.AddInMemoryObject(testClass))
            .ConfigureServices((context, services) =>
            {
                services.AddOptions<TestClass>()
                .BindAsJson(context.Configuration);
            });
        using (var host = hostBuilder.Build())
        {
            await host.StartAsync();
            var options = host.Services.GetRequiredService<IOptions<TestClass>>();
            options.Value.Should().BeEquivalentTo(testClass);
            await host.StopAsync();
        }
    }

    [Theory]
    [AutoData]
    public async Task BindAsJson_can_bind_options_with_name(TestClass testClass, string name)
    {
        testClass.NullValue = null;
        testClass.NullList = null;
        var hostBuilder = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(builder => builder.AddInMemoryObject(testClass))
            .ConfigureServices((context, services) =>
            {
                services.AddOptions<TestClass>(name)
                .BindAsJson(context.Configuration);
            });
        using (var host = hostBuilder.Build())
        {
            await host.StartAsync();
            var options = host.Services.GetRequiredService<IOptionsMonitor<TestClass>>();
            options.Get(name).Should().BeEquivalentTo(testClass);
            await host.StopAsync();
        }
    }
}
