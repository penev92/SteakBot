using System.Threading.Tasks;
using Discord.WebSocket;

namespace SteakBot.Core.Abstractions.EventHandlers
{
    public interface IMessageEventHandler
    {
        Task HandleMessageReceivedAsync(SocketMessage messageParam);
    }
}
