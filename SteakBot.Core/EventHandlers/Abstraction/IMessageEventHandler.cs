using System.Threading.Tasks;
using Discord.WebSocket;

namespace SteakBot.Core.EventHandlers.Abstraction
{
	public interface IMessageEventHandler
	{
		Task HandleMessageReceivedAsync(SocketMessage messageParam);
	}
}
