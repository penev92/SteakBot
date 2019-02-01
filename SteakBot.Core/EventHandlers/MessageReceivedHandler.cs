using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace SteakBot.Core.EventHandlers
{
	internal class MessageReceivedHandler
	{
		private readonly DiscordSocketClient _client;
		private readonly CommandService _commands;
		private readonly IServiceProvider _serviceProvider;

		internal MessageReceivedHandler(DiscordSocketClient client, CommandService commands, IServiceProvider serviceProvider)
		{
			_client = client;
			_commands = commands;
			_serviceProvider = serviceProvider;
		}

		internal async Task HandleMessageReceivedAsync(SocketMessage messageParam)
		{
			// Don't process the command if it was a System Message
			var message = messageParam as SocketUserMessage;
			if (message == null)
				return;

			if (message.Source == MessageSource.Bot)
				return;

			// Create a number to track where the prefix ends and the command begins
			var argPos = 0;

			// Create a Command Context
			var context = new SocketCommandContext(_client, message);

			// Determine if the message is a command, based on if it starts with '!' or a mention prefix
			if (message.HasCharPrefix('!', ref argPos))// || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
			{
				if (await ManualCommandHandler.HandleCommandAsync(message))
				{
					return;
				}

				// Execute the command. (result does not indicate a return value, 
				// rather an object stating if the command executed successfully)
				var result = await _commands.ExecuteAsync(context, argPos, _serviceProvider);
				if (!result.IsSuccess)
				{
					await context.Channel.SendMessageAsync(result.ErrorReason);
				}
			}
		}
	}
}
