using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using SteakBot.Core.Modules;
using SteakBot.Core.Objects;
using SteakBot.Core.Objects.Enums;

namespace SteakBot.Core.EventHandlers.CustomMessageHandlers.CommandMessageHandlers
{
    public  class CustomCommandMessageHandler : BaseCommandMessageHandler
    {
        private readonly MemeService _memeService;

        internal static IList<MemeCommand> Commands { get; set; }

        public CustomCommandMessageHandler(MemeService memeService)
        {
            _memeService = memeService;
            _memeService.ReloadCommandsEvent += ReloadCommands;

            Commands = _memeService.LoadCommands();
            CommandNames = Commands.Select(x => x.Name);
        }

        protected override bool InvokeInner(SocketUserMessage message)
        {
            var channel = message.Channel;
            var commandText = message.Content.Replace(CommandChar, "").Replace(DeleteMessageChar, "");
            var command = Commands.SingleOrDefault(x => x.Name == commandText);
            switch (command?.ResultType)
            {
                case MemeResultType.Text:
                    {
                        channel.SendMessageAsync(command.Value).Wait();
                        return true;
                    }

                case MemeResultType.Image:
                    {
                        var embed = new EmbedBuilder
                        {
                            ImageUrl = command.Value
                        };

                        channel.SendMessageAsync("", embed: embed.Build()).Wait();
                        return true;
                    }

                default:
                    {
                        return false;
                    }
            }
        }

        #region Private methods

        private void ReloadCommands()
        {
            Commands = _memeService.LoadCommands();
            CommandNames = Commands.Select(x => x.Name);
        }

        #endregion
    }
}
