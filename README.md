# asp.net.core.helper

asp.net.core.helper is a library to greatly reduce asp.net boilerplate code and lets you focus on your domain specific stuff.

![Nuget](https://img.shields.io/nuget/v/asp.net.core.helper?logo=NuGet)
![](https://github.com/bPoller2810/asp.net.core.helper/actions/workflows/dotnet.yml/badge.svg)

# Usage

[Startup.cs](sample/helper.sample/Startup.cs): As Last steps before you Map your endpoints, use [MigrateDatabase](src/asp.net.core.helper.core/Seed/Extensions/WebAppExtensions.cs) and/or [SeedDatabase](src/asp.net.core.helper.core/Seed/Extensions/WebAppExtensions.cs) to get the Package working.
```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    ...
    app.UseRouting();

    app.MigrateDatabase<SampleContext>();
    app.SeedDatabase<SampleContext>(typeof(Startup));

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}
```

# MigrateDatabase
Use this Extension to apply your EF Core Migrations properly

# SeedDatabase
### Supported database adapters: (Tested)
- Pomelo.EntityFrameworkCore.MySql (5.0.3) for MariaDb 10..6.4
### Example Seed see [UserSeed.cs](sample/helper.sample/Seeds/UserSeed.cs)
- L7: Inherit from BaseSeed with your Context as generic argument
- L10: Constructor to pass the Injected Context to the BaseClass
- L16:     The unique Key of this seed
- L18:     The Order in wich the seeds should be executed
- L20-36:  This seed adds a default admin user to the Users Table

## ToDo:
- NuGet
- CI / CD
- Document sources and sample
- Microsoft SQL Server IQueryProvider

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)
