using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace SteakBot.Core.EventHandlers
{
	internal class ReactionEventHandler
	{
		internal static async Task HandleReactionAdded(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
		{
			var channel = arg3.Channel;
			var message = await channel.GetMessageAsync(arg3.MessageId);

			await channel.SendMessageAsync($"{arg3.User.Value.Mention} reacted with {arg3.Emote} to a message by {message.Author.Username}"
			                               + $" from {message.CreatedAt.LocalDateTime:HH:mm:ss} of {message.CreatedAt.LocalDateTime:dd.MM.yyyy}.");
		}
	}
}
