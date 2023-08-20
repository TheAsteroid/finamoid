using Finamoid.Cli;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateDefaultBuilder();

builder.ConfigureServices(s =>
{
    s.AddHostedService<CliService>();
});

builder.ConfigureLogging(l => l.SetMinimumLevel(LogLevel.Warning));

using var host = builder.Build();

await host.RunAsync();
