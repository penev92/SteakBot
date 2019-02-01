using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SteakBot.Core.EventHandlers.Abstraction;

namespace SteakBot.Core.EventHandlers
{
	internal class CommandMessageEventHandler : IMessageEventHandler
	{
		private const char CommandChar = '!';

		private readonly DiscordSocketClient _client;
		private readonly CommandService _commands;
		private readonly IServiceProvider _serviceProvider;

		internal CommandMessageEventHandler(DiscordSocketClient client, CommandService commands, IServiceProvider serviceProvider)
		{
			_client = client;
			_commands = commands;
			_serviceProvider = serviceProvider;
		}

		public async Task HandleMessageReceivedAsync(SocketMessage messageParam)
		{
			// Don't process the command if it was a System Message, the sender is a bot or this is not a command.
			if (!(messageParam is SocketUserMessage message) || message.Source == MessageSource.Bot || !IsCommand(message, out var argPos))
			{
				return;
			}

			if (await ManualCommandHandler.HandleCommandAsync(message))
			{
				return;
			}

			var context = new SocketCommandContext(_client, message);
			var result = await _commands.ExecuteAsync(context, argPos, _serviceProvider);
			if (!result.IsSuccess)
			{
				await context.Channel.SendMessageAsync(result.ErrorReason);
			}
		}

		private static bool IsCommand(SocketUserMessage message, out int argumentPosition)
		{
			argumentPosition = 0;
			return message.HasCharPrefix(CommandChar, ref argumentPosition);
		}
	}
}
