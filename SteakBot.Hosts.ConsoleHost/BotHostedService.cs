using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SteakBot.Core.Abstractions;

namespace SteakBot.Hosts.ConsoleHost;

public class BotHostedService : BackgroundService
{
    private readonly IBot _bot;

    public BotHostedService(IBot bot)
    {
        _bot = bot;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bot.RunAsync();
    }
}