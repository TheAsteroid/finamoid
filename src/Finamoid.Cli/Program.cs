using Finamoid.Aggregation;
using Finamoid.Categorization;
using Finamoid.Cli;
using Finamoid.Encryption;
using Finamoid.Import;
using Finamoid.Mutations;
using Finamoid.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder();

builder.Services
    .AddHostedService<CliService>()
    .AddAggregation()
    .AddCategorization()
    .AddEncryption()
    .AddImport()
    .AddMutations()
    .AddStorage(builder.Configuration);

builder.Logging.SetMinimumLevel(LogLevel.Warning);

using var host = builder.Build();

await host.RunAsync();
