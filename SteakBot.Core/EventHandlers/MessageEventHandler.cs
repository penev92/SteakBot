using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using SteakBot.Core.Abstractions.EventHandlers;
using SteakBot.Core.Abstractions.Providers;

namespace SteakBot.Core.EventHandlers
{
    internal class MessageEventHandler : IMessageEventHandler
    {
        private readonly ICustomMessageHandler[] _customMessageHandlers;

        public MessageEventHandler(ICustomMessageHandlerProvider customMessageHandlerProvider)
        {
            _customMessageHandlers = customMessageHandlerProvider.Get().ToArray();
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
