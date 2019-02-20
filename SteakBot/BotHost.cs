using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SteakBot.Core;

namespace SteakBot
{
    internal interface IBotHost
    {
        Task RunAsync();
    }

    internal class BotHost : IBotHost
    {
        private readonly IServiceProvider _serviceProvider;

        public BotHost(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task RunAsync()
        {
            var bot = _serviceProvider.GetService<Bot>();
            await bot.RunAsync();
        }
    }
}
