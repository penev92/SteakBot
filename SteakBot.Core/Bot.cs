using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using SteakBot.Core.Modules;

namespace SteakBot.Core
{
	public class Bot : IDisposable
	{
		private static readonly string discordBotToken = "";

		private readonly DiscordSocketClient _client;
		private readonly CommandService _commands;
		private readonly IServiceProvider _serviceProvider;

		public Bot()
		{
			_client = new DiscordSocketClient();
			_commands = new CommandService();
			_serviceProvider = new ServiceCollection()
				.AddSingleton(_client)
				.AddSingleton(_commands)
				.AddSingleton<AudioService>()
				.BuildServiceProvider();

			AttachEventHandlers();
			RegisterCommandModules();
		}

		public async Task RunAsync()
		{
			await _client.LoginAsync(TokenType.Bot, discordBotToken);
			await _client.StartAsync();

			Console.ReadLine();

			await _client.LogoutAsync();
			await _client.StopAsync();

			Console.WriteLine("Done");
			Console.ReadLine();
		}

		private async Task HandleReactionAdded(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
		{
			var channel = arg3.Channel;
			var message = await channel.GetMessageAsync(arg3.MessageId);

			await channel.SendMessageAsync($"{arg3.User.Value.Mention} reacted with {arg3.Emote} to a message by {message.Author.Username}"
				+ $" from {message.CreatedAt.LocalDateTime.ToString("HH:mm:ss")} of {message.CreatedAt.LocalDateTime.ToString("dd.MM.yyyy")}.");
		}

		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}

		private async Task HandleUserVoiceStateUpdated(SocketUser user, SocketVoiceState leaveState, SocketVoiceState joinState)
		{
			if (joinState.VoiceChannel != null)
			{
				// TODO: Perhaps not hardcode this so?
				var channel = joinState.VoiceChannel.Guild.Channels.FirstOrDefault(x => x.Name == "bendoverwatch");
				var messageChannel = channel as ISocketMessageChannel;
				if (messageChannel != null)
				{
					await messageChannel.SendMessageAsync($"{user.Mention} has joined {joinState.VoiceChannel.Name}");
				}
			}
			else
			if (leaveState.VoiceChannel != null)
			{
				// TODO: Perhaps not hardcode this so?
				var channel = leaveState.VoiceChannel.Guild.Channels.FirstOrDefault(x => x.Name == "bendoverwatch");
				var messageChannel = channel as ISocketMessageChannel;
				if (messageChannel != null)
				{
					await messageChannel.SendMessageAsync($"{user.Mention} has ragequit");
				}
			}
		}

		private async Task HandleCommandAsync(SocketMessage messageParam)
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

		public void Dispose()
		{
			((IDisposable) _commands)?.Dispose();
			_client?.Dispose();
		}

		#region Private methods

		private void AttachEventHandlers()
		{
			_client.Log += Log;
			_client.ReactionAdded += HandleReactionAdded;
			_client.MessageReceived += HandleCommandAsync;
			_client.UserVoiceStateUpdated += HandleUserVoiceStateUpdated;
		}

		private void RegisterCommandModules()
		{
			_commands.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider).Wait();
		}

		#endregion
	}
}
