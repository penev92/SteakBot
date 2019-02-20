using System;

namespace SteakBot
{
    internal interface IBotHostBuilder
    {
        IBotHostBuilder UseServiceProvider(IServiceProvider serviceProvider);

        IBotHost Build();
    }

    internal class BotHostBuilder : IBotHostBuilder
    {
        private IServiceProvider _serviceProvider;


        public IBotHostBuilder UseServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            return this;
        }

        public IBotHost Build()
        {
            return new BotHost(_serviceProvider);
        }
    }
}
