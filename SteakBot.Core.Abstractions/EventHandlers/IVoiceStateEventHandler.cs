using System.Threading.Tasks;
using Discord.WebSocket;

namespace SteakBot.Core.Abstractions.EventHandlers
{
    public interface IVoiceStateEventHandler
    {
        Task HandleUserVoiceStateUpdatedAsync(SocketUser user, SocketVoiceState leaveState, SocketVoiceState joinState);
    }
}