using System.Threading.Tasks;
using Discord.WebSocket;

namespace SteakBot.Core.Abstractions.Handlers
{
    public interface IMessageEventHandler
    {
        Task HandleMessageReceivedAsync(SocketMessage messageParam);
    }
}
