﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using SteakBot.Core.Abstractions;
using SteakBot.Core.Abstractions.Configuration;
using SteakBot.Core.Abstractions.EventHandlers;
using SteakBot.Core.TypeReaders;

namespace SteakBot.Core
{
    public class Bot : IBot
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        private readonly ILogEventHandler _logEventHandler;
        private readonly IMessageEventHandler _messageEventHandler;
        private readonly IReactionEventHandler _reactionEventHandler;
        private readonly IVoiceStateEventHandler _voiceStateEventHandler;

        private readonly IBotConfiguration _botConfiguration;

        public Bot(IServiceProvider serviceProvider, IBotConfiguration botConfiguration)
        {
            _serviceProvider = serviceProvider;
            _botConfiguration = botConfiguration;

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
            await _client.LoginAsync(TokenType.Bot, _botConfiguration.BotToken);
            await _client.StartAsync();

            Console.ReadLine();

            await _client.LogoutAsync();
            await _client.StopAsync();

            Console.WriteLine("Done");
            Console.ReadLine();
        }

        public void Dispose()
        {
            ((IDisposable)_commands)?.Dispose();
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
            var typeReaders = _serviceProvider.GetServices<BaseTypeReader>();
            foreach (var typeReader in typeReaders)
            {
                _commands.AddTypeReader(typeReader.SupportedType, typeReader);
            }

            var modules = _serviceProvider.GetServices<ModuleBase<SocketCommandContext>>();
            foreach (var module in modules)
            {
                if (!_botConfiguration.EnabledModules.Contains(module.GetType().Name))
                {
                    continue;
                }

                _commands.AddModuleAsync(module.GetType(), _serviceProvider).Wait();
            }
        }

        #endregion
    }
}
