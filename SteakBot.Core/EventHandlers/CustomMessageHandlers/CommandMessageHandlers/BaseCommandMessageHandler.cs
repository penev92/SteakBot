using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using SteakBot.Core.Abstractions.EventHandlers;

namespace SteakBot.Core.EventHandlers.CustomMessageHandlers.CommandMessageHandlers
{
    internal abstract class BaseCommandMessageHandler : ICustomMessageHandler
    {
        protected IEnumerable<string> CommandNames;

        public bool CanHandle(SocketUserMessage message)
        {
            var messageString = message.Content;
            var commandText = messageString.Replace(GlobalConstants.CommandChar, "").Split(' ', '\n')[0];
            return messageString.StartsWith(GlobalConstants.CommandChar) && CommandNames.Any(x => x == commandText);
        }

        public void Invoke(SocketUserMessage message)
        {
            if (InvokeInner(message) && ShouldDeleteMessage(message))
            {
                DeleteMessageAsync(message).Wait();
            }
        }

        protected abstract bool InvokeInner(SocketUserMessage message);

        protected abstract bool ShouldDeleteMessage(SocketUserMessage message);

        protected virtual async Task DeleteMessageAsync(SocketUserMessage message)
        {
            await message.Channel.DeleteMessageAsync(message, RequestOptions.Default);
        }
    }
}
