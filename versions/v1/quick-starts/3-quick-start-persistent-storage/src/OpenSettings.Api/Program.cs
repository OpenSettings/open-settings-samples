using Microsoft.EntityFrameworkCore;
using OpenSettings.AspNetCore.Extensions;
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
app.UseOpenSettings(); // Updates instance status when the application starts or stops & serve OpenSettings Spa.
app.MapControllers();

await app.RunAsync();

return;

static OpenSettingsConfiguration GetOpenSettingsConfiguration(IConfiguration configuration)
{
    var migrationsAssembly = typeof(Program).Assembly.GetName().Name;

    var openSettingsConfiguration = configuration.GetSection(nameof(OpenSettingsConfiguration)).Get<OpenSettingsConfiguration>();

    openSettingsConfiguration.Provider.Orm.ConfigureDbContext = optsBuilder =>
    {
        optsBuilder.UseSqlServer(configuration["SqlServerConnectionString"], opts => opts.MigrationsAssembly(migrationsAssembly));
    };

    return openSettingsConfiguration;
}