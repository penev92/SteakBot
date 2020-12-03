using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SteakBot.Core;
using SteakBot.Core.DependencyInjection;

namespace SteakBot
{
    internal class Program
    {
        private static async Task Main()
        {
            using var serviceProvider = new ServiceCollection()
                .AddSingleton<Bot>()
                .AddBasicDiscordServices()
                .AddDefaultEventHandlerServices()
                .AddCustomTypeReaders()
                .AddDefaultModules()
                .AddDefaultCustomMessageHandlers()
                .AddGitHubIntegrationServices()
                .AddBitBucketIntegrationServices()
                .BuildServiceProvider();

            await serviceProvider
                .GetService<Bot>()
                .RunAsync();
        }
    }
}
