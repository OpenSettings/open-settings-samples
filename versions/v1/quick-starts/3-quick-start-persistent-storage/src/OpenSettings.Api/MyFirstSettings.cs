using OpenSettings.Services.Interfaces;

namespace OpenSettings.Api
{
    public class MyFirstSettings : ISettings
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}