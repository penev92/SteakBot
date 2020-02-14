using System.Linq;
using Discord;
using Discord.WebSocket;
using SteakBot.Core.Services;
using SteakBot.Core.Objects.Enums;

namespace SteakBot.Core.EventHandlers.CustomMessageHandlers.CommandMessageHandlers
{
    internal class CustomCommandMessageHandler : BaseCommandMessageHandler
    {
        private readonly MemeService _memeService;

        public CustomCommandMessageHandler(MemeService memeService)
        {
            _memeService = memeService;
            _memeService.OnReloadCommands += OnReloadCommandNames;
            OnReloadCommandNames();
        }

        protected override bool InvokeInner(SocketUserMessage message)
        {
            var channel = message.Channel;
            var messageText = message.Content;

            var startIndex = 0;
            var endIndex = messageText.Length;
            if (messageText.StartsWith(GlobalConstants.CommandChar))
            {
                startIndex++;
            }
            if (messageText.EndsWith(GlobalConstants.DeleteMessageChar))
            {
                endIndex--;
            }
            var commandName = messageText.Substring(startIndex, endIndex);

            var command = _memeService.MemeCommands.Single(x => x.Name == commandName);
            switch (command.ResultType)
            {
                case MemeResultType.Text:
                case MemeResultType.Video:
                    channel.SendMessageAsync($"{message.Author.Username}: {command.Value}").Wait();
                    return true;

                case MemeResultType.Image:
                    var embed = new EmbedBuilder()
                    {
                        ImageUrl = command.Value
                    };
                    channel.SendMessageAsync($"{message.Author.Username}:", embed: embed.Build()).Wait();
                    return true;

                default:
                    return false;
            }
        }

        protected override bool ShouldDeleteMessage(SocketUserMessage message)
        {
            return true;
        }

        #region Private methods

        private void OnReloadCommandNames()
        {
            CommandNames = _memeService.MemeCommands.Select(x => x.Name);
        }

        #endregion
    }
}
