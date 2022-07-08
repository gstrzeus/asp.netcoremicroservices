using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hostctn, config) =>
{
    config.AddJsonFile($"ocelot.{hostctn.HostingEnvironment.EnvironmentName}.json", true, true);
});

builder.Host.ConfigureLogging((hostCtx, logginBuilder) =>
{
    logginBuilder.AddConfiguration(hostCtx.Configuration.GetSection("Logging"));
    logginBuilder.AddConsole();
    logginBuilder.AddDebug();
});

builder.Services.AddOcelot()
    .AddCacheManager(x => x.WithDictionaryHandle());

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

await app.UseOcelot();

app.Run();
