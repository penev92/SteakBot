using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using SteakBot.Core.Abstractions;
using SteakBot.Core.Abstractions.Handlers;
using SteakBot.Core.Abstractions.Options;
using SteakBot.Core.TypeReaders;

namespace SteakBot.Core
{
    public class Bot : IDisposable
    {
        private readonly IBotOptions _botOptions;

        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        private readonly ILogEventHandler _logEventHandler;
        private readonly IMessageEventHandler _messageEventHandler;
        private readonly IReactionEventHandler _reactionEventHandler;
        private readonly IVoiceStateEventHandler _voiceStateEventHandler;
        private readonly IEnumerable<BaseTypeReader> _typeReaders;
        private readonly IEnumerable<ModuleBase<SocketCommandContext>> _modules;
        private IServiceProvider _serviceProvider;

        public Bot(DiscordSocketClient client,
            CommandService commands,
            ILogEventHandler logEventHandler,
            IMessageEventHandler messageEventHandler,
            IReactionEventHandler reactionEventHandler,
            IVoiceStateEventHandler voiceStateEventHandler, 
            IEnumerable<ModuleBase<SocketCommandContext>> modules,
            IEnumerable<BaseTypeReader> typeReaders,
            IServiceProvider serviceProvider, 
            IBotOptions botOptions)
        {
            _client = client;
            _commands = commands;
            _logEventHandler = logEventHandler;
            _messageEventHandler = messageEventHandler;
            _reactionEventHandler = reactionEventHandler;
            _voiceStateEventHandler = voiceStateEventHandler;
            _modules = modules;
            _typeReaders = typeReaders;
            _serviceProvider = serviceProvider;
            _botOptions = botOptions;

            AttachEventHandlers();
            RegisterCommandModules();
        }

        public async Task RunAsync()
        {
            await _client.LoginAsync(TokenType.Bot, _botOptions.BotToken);
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
            foreach (var typeReader in _typeReaders)
            {
                _commands.AddTypeReader(typeReader.SupportedType, typeReader);
            }

            foreach (var module in _modules)
            {
                _commands.AddModuleAsync(module.GetType(), _serviceProvider).Wait();
            }
        }

        #endregion
    }
}
