using Discord.WebSocket;

namespace SteakBot.Core.Abstractions.EventHandlers
{
    public interface ICustomMessageHandler
    {
        bool CanHandle(SocketUserMessage message);

        void Invoke(SocketUserMessage message);
    }
}
