using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using SteakBot.Core.Abstractions.EventHandlers;

namespace SteakBot.Core.EventHandlers
{
    internal class MessageEventHandler : IMessageEventHandler
    {
        private readonly IEnumerable<ICustomMessageHandler> _customMessageHandlers;

        public MessageEventHandler(IServiceProvider serviceProvider)
        {
            _customMessageHandlers = serviceProvider.GetServices<ICustomMessageHandler>();
        }

        public async Task HandleMessageReceivedAsync(SocketMessage messageParam)
        {
            // Process the command if it is a User Message and the sender is not a bot.
            if (messageParam is SocketUserMessage message && messageParam.Source != MessageSource.Bot)
            {
                Parallel.ForEach(_customMessageHandlers, customMessageHandler =>
                {
                    if (customMessageHandler.CanHandle(message))
                    {
                        customMessageHandler.Invoke(message);
                    }
                });
            }

            await Task.CompletedTask;
        }
    }
}
