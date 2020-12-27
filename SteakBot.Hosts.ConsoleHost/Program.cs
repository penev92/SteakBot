using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SteakBot.Core.Abstractions;
using SteakBot.Core.DependencyInjection;

namespace SteakBot.Hosts.ConsoleHost
{
    internal class Program
    {
        private static async Task Main()
        {
            await using var serviceProvider = new ServiceCollection()
                .AddAppSettingsConfiguration()
                .AddBasicDiscordServices()
                .AddDefaultDiscordBot()
                .AddDefaultEventHandlerServices()
                .AddCustomTypeReaders()
                .AddDefaultModules()
                .AddDefaultCustomMessageHandlers()
                .AddGitHubIntegrationServices()
                .AddBitBucketIntegrationServices()
                .BuildServiceProvider();

            await serviceProvider
                .GetService<IBot>()
                .RunAsync();
        }
    }
}
