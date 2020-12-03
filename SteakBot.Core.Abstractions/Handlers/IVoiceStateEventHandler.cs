using System.Threading.Tasks;
using Discord.WebSocket;

namespace SteakBot.Core.Abstractions.Handlers
{
    public interface IVoiceStateEventHandler
    {
        Task HandleUserVoiceStateUpdatedAsync(SocketUser user, 
            SocketVoiceState leaveState,
            SocketVoiceState joinState);
    }
}