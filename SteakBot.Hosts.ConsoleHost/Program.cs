using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SteakBot.Core.DependencyInjection;

namespace SteakBot.Hosts.ConsoleHost
{
    internal class Program
    {
        private static async Task Main()
        {
            var hostBuilder = Host
                .CreateDefaultBuilder()
                .ConfigureServices(
                    services =>
                    {
                        services
                            .AddAppSettingsConfiguration()
                            .AddBasicDiscordServices()
                            .AddDefaultDiscordBot()
                            .AddDefaultEventHandlerServices()
                            .AddCustomTypeReaders()
                            .AddDefaultModules()
                            .AddDefaultCustomMessageHandlers()
                            .AddGitHubIntegrationServices()
                            .AddBitBucketIntegrationServices()
                            .AddHostedService<BotHostedService>();
                    });

            using var host = hostBuilder.Build();
            await host.RunAsync();
        }
    }
}
