using System;
using System.Linq;
using Discord.Commands;
using Discord.WebSocket;

namespace SteakBot.Core.EventHandlers.CustomMessageHandlers.CommandMessageHandlers
{
    internal class StandardCommandMessageHandler : BaseCommandMessageHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _serviceProvider;

        private readonly char _commandChar;

        public StandardCommandMessageHandler(DiscordSocketClient client, CommandService commands, IServiceProvider serviceProvider)
        {
            _client = client;
            _commands = commands;
            _serviceProvider = serviceProvider;
            _commandChar = GlobalConstants.CommandChar[0];
            CommandNames = _commands.Commands.Select(x => x.Name);
        }

        protected override bool InvokeInner(SocketUserMessage message)
        {
            var argumentPosition = 0;
            message.HasCharPrefix(_commandChar, ref argumentPosition);

            var context = new SocketCommandContext(_client, message);

            var result = _commands.ExecuteAsync(context, argumentPosition, _serviceProvider).Result;
            if (!result.IsSuccess)
            {
                context.Channel.SendMessageAsync(result.ErrorReason);
            }

            return result.IsSuccess;
        }

        protected override bool ShouldDeleteMessage(SocketUserMessage message)
        {
            return message.Content.EndsWith(GlobalConstants.DeleteMessageChar);
        }
    }
}
