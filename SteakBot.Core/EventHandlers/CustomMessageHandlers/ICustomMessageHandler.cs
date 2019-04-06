using Discord.WebSocket;

namespace SteakBot.Core.EventHandlers.CustomMessageHandlers
{
    public interface ICustomMessageHandler
    {
        bool CanHandle(SocketUserMessage message);

        void Invoke(SocketUserMessage message);
    }
}
