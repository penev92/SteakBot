using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace SteakBot.Core.EventHandlers.Abstraction
{
	internal interface IReactionEventHandler
	{
		Task HandleReactionAddedAsync(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3);
	}
}