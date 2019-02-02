using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using SteakBot.Core.EventHandlers;
using SteakBot.Core.EventHandlers.Abstraction;
using SteakBot.Core.Modules;

namespace SteakBot.Core
{
	public class Bot : IDisposable
	{
		private static readonly string discordBotToken = "";

		private readonly DiscordSocketClient _client;
		private readonly CommandService _commands;
		private readonly IServiceProvider _serviceProvider;

		private readonly ILogEventHandler _logEventHandler;
		private readonly IMessageEventHandler _messageEventHandler;
		private readonly IReactionEventHandler _reactionEventHandler;
		private readonly IVoiceStateEventHandler _voiceStateEventHandler;

		public Bot()
		{
			_serviceProvider = new ServiceCollection()
				.AddSingleton<DiscordSocketClient>()
				.AddSingleton<CommandService>()
				.AddSingleton<ILogEventHandler, LogEventHandler>()
				.AddSingleton<IMessageEventHandler, MessageEventHandler>()
				.AddSingleton<IReactionEventHandler, ReactionEventHandler>()
				.AddSingleton<IVoiceStateEventHandler, VoiceStateEventHandler>()
				.AddSingleton<AudioService>()
				.BuildServiceProvider();

			_client = _serviceProvider.GetService<DiscordSocketClient>();
			_commands = _serviceProvider.GetService<CommandService>();
			_logEventHandler = _serviceProvider.GetService<ILogEventHandler>();
			_messageEventHandler = _serviceProvider.GetService<IMessageEventHandler>();
			_reactionEventHandler = _serviceProvider.GetService<IReactionEventHandler>();
			_voiceStateEventHandler = _serviceProvider.GetService<IVoiceStateEventHandler>();

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
			_client.Log += _logEventHandler.Log;
			_client.ReactionAdded += _reactionEventHandler.HandleReactionAddedAsync;
			_client.MessageReceived += _messageEventHandler.HandleMessageReceivedAsync;
			_client.UserVoiceStateUpdated += _voiceStateEventHandler.HandleUserVoiceStateUpdatedAsync;
		}

		private void RegisterCommandModules()
		{
			_commands.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider).Wait();
		}

		#endregion
	}
}
