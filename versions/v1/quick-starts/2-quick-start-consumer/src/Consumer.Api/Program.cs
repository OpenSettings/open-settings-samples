using OpenSettings.AspNetCore;
using OpenSettings.Configurations;
using OpenSettings.Extensions;
using OpenSettings.Models;

var builder = WebApplication.CreateBuilder(args);

var openSettingsConsumerConfiguration = new OpenSettingsConfiguration(ServiceType.Consumer)
{
    Client = new ClientInfo(
        new Guid("71059bda-bb49-447f-ac83-60cd15c9518d"), // The unique identifier for the client.
        new Guid("6c52c9f7-d43c-44c1-8d6c-451bf9029731")  // The secret key for the client.
    ),
    Consumer = new ConsumerConfiguration
    {
        ProviderUrl = "http://localhost:5288/api/settings" // Url of the provider service.
    }
};

await builder.Host.UseOpenSettingsAsync(openSettingsConsumerConfiguration); // Registers OpenSettings

builder.Services
    .AddControllers()
    .AddOpenSettingsController(builder.Configuration); // Enables OpenSettings Controllers

var app = builder.Build();

app.UseRouting();
app.UseOpenSettings(); // Updates instance status when the application is starting or stopping.
app.UseOpenSettingsSpa(); // Enables OpenSettings Spa page for viewing and editing settings.
app.MapControllers();

await app.RunAsync();