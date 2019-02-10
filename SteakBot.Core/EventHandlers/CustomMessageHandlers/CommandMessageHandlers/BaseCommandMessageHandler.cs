using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace SteakBot.Core.EventHandlers.CustomMessageHandlers.CommandMessageHandlers
{
	internal abstract class BaseCommandMessageHandler : ICustomMessageHandler
	{
		protected const string CommandChar = "!";
		protected const string DeleteMessageChar = "!";

		protected IEnumerable<string> CommandNames;

		public bool CanHandle(SocketUserMessage message)
		{
			var messageString = message.Content;
			var commandText = messageString.Replace(CommandChar, "").Replace(DeleteMessageChar, "").Split(' ', '\n')[0];
			return messageString.StartsWith(CommandChar) && CommandNames.Any(x => x == commandText);
		}

		public void Invoke(SocketUserMessage message)
		{
			if (InvokeInner(message) && ShouldDeleteMessage(message))
			{
				DeleteMessageAsync(message).Wait();
			}
		}

		protected abstract bool InvokeInner(SocketUserMessage message);

		protected virtual bool ShouldDeleteMessage(SocketUserMessage message)
		{
			return message.Content.EndsWith(DeleteMessageChar);
		}

		protected virtual async Task DeleteMessageAsync(SocketUserMessage message)
		{
			await message.Channel.DeleteMessageAsync(message, RequestOptions.Default);
		}
	}
}
