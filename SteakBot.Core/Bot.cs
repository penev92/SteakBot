using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using SteakBot.Core.EventHandlers;
using SteakBot.Core.Modules;

namespace SteakBot.Core
{
	public class Bot : IDisposable
	{
		private static readonly string discordBotToken = "";

		private readonly DiscordSocketClient _client;
		private readonly CommandService _commands;
		private readonly IServiceProvider _serviceProvider;
		private readonly CommandMessageEventHandler _messageEventHandler;

		public Bot()
		{
			_client = new DiscordSocketClient();
			_commands = new CommandService();
			_serviceProvider = new ServiceCollection()
				.AddSingleton(_client)
				.AddSingleton(_commands)
				.AddSingleton<AudioService>()
				.BuildServiceProvider();

			_messageEventHandler = new CommandMessageEventHandler(_client, _commands, _serviceProvider);

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

		public void Dispose()
		{
			((IDisposable) _commands)?.Dispose();
			_client?.Dispose();
		}

		#region Private methods

		private void AttachEventHandlers()
		{
			_client.Log += LogHandler.Log;
			_client.ReactionAdded += ReactionEventHandler.HandleReactionAdded;
			_client.MessageReceived += _messageEventHandler.HandleMessageReceivedAsync;
			_client.UserVoiceStateUpdated += VoiceStateEventHandler.HandleUserVoiceStateUpdated;
		}

		private void RegisterCommandModules()
		{
			_commands.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider).Wait();
		}

		#endregion
	}
}
