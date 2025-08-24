using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ogu.Compressions.Abstractions;
using OpenSettings.AspNetCore.Extensions;
using OpenSettings.Configurations;
using OpenSettings.Extensions;
using OpenSettings.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

await builder.Host.UseOpenSettingsAsync(ConsumerConfiguration()); // Registers OpenSettings

builder.Services
    .AddControllers()
    .AddOpenSettingsController(builder.Configuration); // Enables OpenSettings Controllers

var app = builder.Build();

app.UseRouting();
app.UseOpenSettings(); // Updates instance status when the application is starting or stopping & serve OpenSettings Spa.
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();

return;

static OpenSettingsConfiguration ConsumerConfiguration() => new OpenSettingsConfiguration(ServiceType.Consumer)
{
    InstanceName = "consumer-1",
    IdentifierName = "Production",
    Client = new ClientInfo(new Guid("adbdf741-bb4d-4673-b2a8-23e677fcf454"), new Guid("4294a5e3-0839-4358-a03d-1ac52585ae5f")),
    Selection = ServiceType.Consumer,
    Consumer = new ConsumerConfiguration
    {
        ProviderUrl = "http://localhost:5002/api/settings", // Consumer requires provider url for fetching and syncing the data.
        RequestEncodings = { CompressionType.Brotli }, // CompressionTypes int[] -> [0 (None), 1 (Snappy) ,2 (Deflate), 3 (Gzip), 4 (Zstd), 5 (Brotli) ]
        IsRedisActive = false,
        SkipInitialSyncAppData = false,
        PollingSettingsWorker = new PollingSettingsWorkerConfiguration(isActive: true, startsIn: TimeSpan.FromMinutes(1), period: TimeSpan.FromMinutes(5))
    },
    SyncAppDataResilience = new SyncAppDataResilienceConfiguration
    {
        MaxRetryAttempts = -1, // Infinite retries
        RetryDelay = TimeSpan.FromSeconds(1) // Delay in milliseconds between retry attempts
    },
    Operation = Operation.ReadOrInitialize,
    StoreInSeparateFile = true,
    IgnoreOnFileChange = false,
    RegistrationMode = RegistrationMode.Configure
};