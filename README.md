# Extensions for [Microsoft.Extensions](https://github.com/dotnet/extensions)

[![Build Status](https://iron9light.visualstudio.com/github/_apis/build/status/iron9light.IL.Extensions?branchName=master)](https://iron9light.visualstudio.com/github/_build/latest?definitionId=4&branchName=master)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=iron9light_IL.Extensions&metric=ncloc)](https://sonarcloud.io/dashboard?id=iron9light_IL.Extensions)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=iron9light_IL.Extensions&metric=coverage)](https://sonarcloud.io/dashboard?id=iron9light_IL.Extensions)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=iron9light_IL.Extensions&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=iron9light_IL.Extensions)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=iron9light_IL.Extensions&metric=security_rating)](https://sonarcloud.io/dashboard?id=iron9light_IL.Extensions)

## IL.Extensions.Configuration.Memory.NewtonsoftJson

Create [MemoryConfigurationProvider](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?#memory-configuration-provider) with JSON string or JSON serializable object for [Microsoft.Extensions.Configuration](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/) using [Json.NET](https://www.newtonsoft.com/json).

[![NuGet](https://img.shields.io/nuget/vpre/IL.Extensions.Configuration.Memory.NewtonsoftJson.svg)](https://www.nuget.org/packages/IL.Extensions.Configuration.Memory.NewtonsoftJson/)

Before:

```csharp
configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
{
    { "A:B", "100" },
    { "A:C:D", "xx" },
    { "A:C:E", "2020-03-29T14:22:30.000Z" },
})
```

Now:

```csharp
configurationBuilder.AddInMemoryObject(new
{
    A = new
    {
        B = 100,
        C = new
        {
            D = "xx",
            E = DateTimeOffset.Parse("2020-03-29T14:22:30.000Z"),
        },
    },
})
```

The object will convert to `JToken` first and then convert to `IEnumerable<KeyValuePair<string, string>> initialData` for `AddInMemoryCollection`.

## IL.Extensions.Configuration.Binder.NewtonsoftJson

Functionality to bind an object to data in configuration providers for [Microsoft.Extensions.Configuration](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/) using [Json.NET](https://www.newtonsoft.com/json).

It's an alternative for [Microsoft.Extensions.Configuration.Binder](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Binder/).

[![NuGet](https://img.shields.io/nuget/vpre/IL.Extensions.Configuration.Binder.NewtonsoftJson.svg)](https://www.nuget.org/packages/IL.Extensions.Configuration.Binder.NewtonsoftJson/)

Convert `IConfiguration` to `JToken`.

```csharp
var jToken = configuration.ToJToken();
```

Then deserialize it to object using [Json.NET](https://www.newtonsoft.com/json):

```csharp
var myObject = configuration.ToObject<MyObject>();

var myString = configuration.ToObject<string>("aKey", defaultValue: string.Empty);
```

Or populate it to an existing object using [Json.NET](https://www.newtonsoft.com/json):

```csharp
MyObject myObject = new MyObject();
configuration.Populate(myObject);
```

You can use these methods as alternatives for [`ConfigurationBinder.Get<T>(this IConfiguration configuration)`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbinder.get?#Microsoft_Extensions_Configuration_ConfigurationBinder_Get__1_Microsoft_Extensions_Configuration_IConfiguration_), [`ConfigurationBinder.GetValue<T>(this IConfiguration configuration, string key, T defaultValue)`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbinder.getvalue?#Microsoft_Extensions_Configuration_ConfigurationBinder_GetValue__1_Microsoft_Extensions_Configuration_IConfiguration_System_String___0_) and [`ConfigurationBinder.Bind(this IConfiguration configuration, object instance)`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbinder.bind?#Microsoft_Extensions_Configuration_ConfigurationBinder_Bind_Microsoft_Extensions_Configuration_IConfiguration_System_Object_), using popular [Json.NET](https://www.newtonsoft.com/json).

## IL.Extensions.Options.ConfigurationExtensions.NewtonsoftJson

Provides additional configuration specific functionality related to [Microsoft.Extensions.Options](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options) using [Json.NET](https://www.newtonsoft.com/json).

It's an alternative for [Microsoft.Extensions.Options.ConfigurationExtensions](https://www.nuget.org/packages/Microsoft.Extensions.Options.ConfigurationExtensions/).

[![NuGet](https://img.shields.io/nuget/vpre/IL.Extensions.Options.ConfigurationExtensions.NewtonsoftJson.svg)](https://www.nuget.org/packages/IL.Extensions.Options.ConfigurationExtensions.NewtonsoftJson/)

To [register the Configuration instance which MyOptions binds against](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?#general-options-configuration).

Before:

```csharp
services.Configure<MyOptions>(Configuration);
```

Now:

```csharp
services.ConfigureAsJson<MyOptions>(Configuration);
```

It does the same thing, but using [Json.NET](https://www.newtonsoft.com/json) as binding engine.

Or:

```csharp
services.AddOptions<MyOptions>().BindAsJson(Configuration);
```
