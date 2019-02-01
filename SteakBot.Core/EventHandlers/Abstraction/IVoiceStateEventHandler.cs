using System.Threading.Tasks;
using Discord.WebSocket;

namespace SteakBot.Core.EventHandlers.Abstraction
{
	internal interface IVoiceStateEventHandler
	{
		Task HandleUserVoiceStateUpdatedAsync(SocketUser user, SocketVoiceState leaveState, SocketVoiceState joinState);
	}
}