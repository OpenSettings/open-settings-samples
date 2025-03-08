using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ogu.Compressions.Abstractions;
using OpenSettings.AspNetCore;
using OpenSettings.Configurations;
using OpenSettings.Extensions;
using OpenSettings.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

var openSettingsProviderConfiguration = new OpenSettingsConfiguration(ServiceType.Provider)
{
    Client = new ClientInfo(
        new Guid("adbdf741-bb4d-4673-b2a8-23e677fcf454"), // The unique identifier for the client. 
        new Guid("4294a5e3-0839-4358-a03d-1ac52585ae5f")), // The secret key for the client.
};

openSettingsProviderConfiguration.Provider.Orm.ConfigureDbContext = optsBuilder =>
{
    // Configure your database provider here. (e.g. UseSqlServer, UseNpgsql, UseInMemoryDatabase)
    optsBuilder.UseInMemoryDatabase("OpenSettings");
};

await builder.Host.UseOpenSettingsAsync(openSettingsProviderConfiguration); // Registers OpenSettings

builder.Services
    .AddControllers()
    .AddOpenSettingsController(builder.Configuration); // Enables OpenSettings Controllers

var app = builder.Build();

app.UseRouting();
app.UseOpenSettings(); // Updates instance status when the application is starting or stopping.
app.UseOpenSettingsSpa(); // Enables OpenSettings Spa page for viewing and editing settings.
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
    SyncAppDataMaxRetryCount = -1, // Infinite retries
    SyncAppDataRetryDelayMilliseconds = 1000, // Delay in milliseconds between retry attempts
    AllowAnonymousAccess = true,
    Operation = Operation.ReadOrInitialize,
    StoreInSeparateFile = true,
    IgnoreOnFileChange = false,
    RegistrationMode = RegistrationMode.Configure
};