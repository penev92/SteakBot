﻿using Microsoft.Extensions.DependencyInjection;
using SteakBot.Core;
using SteakBot.Core.DependencyInjection;

namespace SteakBot
{
    internal class Program
    {
        private static void Main()
        {
            var serviceProvider = new ServiceCollection()
                .AddBasicDiscordServices()
                .AddDefaultEventHandlerServices()
                .AddCustomTypeReaders()
                .AddDefaultModules()
                .AddDefaultCustomMessageHandlers()
                .AddGitHubIntegrationServices()
                .AddBitBucketIntegrationServices()
                .BuildServiceProvider();

            using (var bot = new Bot(serviceProvider))
            {
                bot.RunAsync().Wait();
            }
        }
    }
}
