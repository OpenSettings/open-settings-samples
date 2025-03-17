using Microsoft.EntityFrameworkCore;
using OpenSettings.AspNetCore;
using OpenSettings.Configurations;
using OpenSettings.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{OpenSettings.Helpers.Helper.GetEnvironmentName()}.json", optional: true)
    .Build();

var openSettingsProviderConfiguration = GetOpenSettingsConfiguration(configuration);

await builder.Host.UseOpenSettingsAsync(openSettingsProviderConfiguration); // Registers OpenSettings

builder.Services
    .AddControllers()
    .AddOpenSettingsController(builder.Configuration); // Enables OpenSettings Controllers

var app = builder.Build();

app.UseRouting();
app.UseOpenSettings(); // Updates instance status when the application is starting or stopping.
app.UseOpenSettingsSpa(); // Enables OpenSettings Spa page for viewing and editing settings.
app.MapControllers();

await app.RunAsync();

return;

static OpenSettingsConfiguration GetOpenSettingsConfiguration(IConfiguration configuration)
{
    var migrationsAssembly = typeof(Program).Assembly.GetName().Name;

    var settingsServiceConfiguration = configuration.GetSection(nameof(OpenSettingsConfiguration)).Get<OpenSettingsConfiguration>();

    settingsServiceConfiguration.Provider.Orm.ConfigureDbContext = optsBuilder =>
    {
        optsBuilder.UseSqlServer(configuration["SqlServerConnectionString"], opts => opts.MigrationsAssembly(migrationsAssembly));
    };

    return settingsServiceConfiguration;
}