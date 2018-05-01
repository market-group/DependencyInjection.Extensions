# Market.Extensions.DependencyInjection

[![Build Status](https://travis-ci.org/market-group/DependencyInjection.Extensions.svg?branch=master)](https://travis-ci.org/market-group/DependencyInjection.Extensions)

|                            |                Stable                     |                 Pre-release               |
| -------------------------: | :---------------------------------------: | :---------------------------------------: |
|                  **Market.Extensions.DependencyInjection**  |     ![nuget-extensions-stable](https://img.shields.io/nuget/v/Market.Extensions.DependencyInjection.svg)           |    ![nuget-extensions-stable](https://img.shields.io/nuget/vpre/Market.Extensions.DependencyInjection.svg)   |


A package that contains extensions on top of [Microsoft.Extensions.DependencyInjection](https://github.com/aspnet/DependencyInjection) provided by the ASP.NET Core Team

## Initialization Extensions

This set of extensions allows to add to the di container singletons that should be initialized on startup.   
(e.g before the first request)

### Constrctor based initialization

```csharp
class MyService
{
    public MyService()
    {
        //Will be called when initializing IServiceProvider
    }
}

class Startup
{
        public void ConfigureServices(IServiceCollection services)
        {
            ....
            ....

            services.AddSingeltonWithInit<MyService>();
            
            ....
            ....
        }

        public void Configure(IApplicationBuilder applicationBuilder)
        {
            ....
            ....

            applicationBuilder.ApplicationServices.Init();
        }
}
```

### `IInitializable` based initialization

It is also possible to initialize a service not just with the `ctor` but also with a dedicated initialization method.
To use this type of initialization simply implement the interface `IInitializable` and register your service in the DI container using 
`AddSingeltonWithInit(...)` extension method

```csharp
class MyService : IInitializable
{
    public MyService()
    {
        //Will be called when initializing IServiceProvider
    }

    public Task InitializeAsync(CancellationToken cancellationToken)
    {
        //Some IO based initialization should go here
    }
}
```

**Note:** `InitializeAsync` can be called multiple times (e.g `serviceProvider.Init()`), it is the resposiblity of the service to gurantee one time initialization.
