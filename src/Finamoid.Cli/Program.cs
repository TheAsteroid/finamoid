using Finamoid.Cli;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Finamoid.Import;
using Microsoft.Extensions.Configuration;
using Finamoid.Storage;
using Finamoid.Encryption;

var builder = Host.CreateApplicationBuilder();

//builder.Configuration
//    .AddJsonFile("appsettings.json", optional: true)
//    .AddEnvironmentVariables()
//    .Build();

builder.Services
    .AddHostedService<CliService>()
    .AddMutationImport()
    .AddEncryption()
    .AddStorage()
    .Configure<StorageOptions>(builder.Configuration.GetSection("Storage"));

builder.Logging.SetMinimumLevel(LogLevel.Warning);

using var host = builder.Build();

await host.RunAsync();
