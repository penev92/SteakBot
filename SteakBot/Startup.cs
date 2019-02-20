using System;
using Microsoft.Extensions.DependencyInjection;
using SteakBot.Core.DependencyInjection;

namespace SteakBot
{
    internal class Startup : IDisposable
    {
        public ServiceProvider ServiceProvider { get; private set; }

        public void ConfigureServices()
        {
            var serviceCollection = new ServiceCollection()
                .AddBasicExternalDiscordServices()
                .AddBasicDiscordServices()
                .AddDefaultEventHandlerServices()
                .AddDefaultModules()
                .AddDefaultCustomMessageHandlers();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public void Dispose()
        {
            ServiceProvider?.Dispose();
            ServiceProvider = null;
        }
    }
}
