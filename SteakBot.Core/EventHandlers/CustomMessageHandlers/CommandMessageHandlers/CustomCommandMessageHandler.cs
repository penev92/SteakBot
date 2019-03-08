using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using SteakBot.Core.Objects;
using SteakBot.Core.Objects.Enums;

namespace SteakBot.Core.EventHandlers.CustomMessageHandlers.CommandMessageHandlers
{
    internal class CustomCommandMessageHandler : BaseCommandMessageHandler
    {
        private static readonly string MemeCommandsFileName = ConfigurationManager.AppSettings["memeCommandsRelativeFilePath"];
        private static readonly string MemeCommandsOriginRelativeFilePath = ConfigurationManager.AppSettings["memeCommandsOriginRelativeFilePath"];

        internal static IList<MemeCommand> Commands { get; set; } = LoadCommands();

        private static IEnumerable<string> commandNames;

        protected override IEnumerable<string> CommandNames { get => commandNames; set => commandNames = value; }

        public CustomCommandMessageHandler()
        {
            commandNames = Commands.Select(x => x.Name);
        }

        public static bool SaveCommand(MemeCommand newCmd)
        {
            if (ValidateNewCommand(newCmd))
            {
                Commands.Add(newCmd);
                using (var fileWriter = new StreamWriter(MemeCommandsFileName))
                {
                    using (var jsonTextWriter = new JsonTextWriter(fileWriter))
                    {
                        var jsonSerializer = new JsonSerializer();
                        jsonSerializer.Serialize(jsonTextWriter, Commands);
                    }
                }

#if DEBUG
                // replace original file
                using (var fileWriter = new StreamWriter(MemeCommandsOriginRelativeFilePath))
                {
                    using (var jsonTextWriter = new JsonTextWriter(fileWriter))
                    {
                        var jsonSerializer = new JsonSerializer();
                        jsonSerializer.Serialize(jsonTextWriter, Commands);
                    }
                }
#endif

                ReloadCommands();

                return true;
            }

            return false;
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

        private static IList<MemeCommand> LoadCommands()
        {
            using (var fileReader = new StreamReader(MemeCommandsFileName))
            {
                using (var jsonTextReader = new JsonTextReader(fileReader))
                {
                    var jsonSerializer = new JsonSerializer();
                    return jsonSerializer.Deserialize<IList<MemeCommand>>(jsonTextReader);
                }
            }
        }

        private static void ReloadCommands()
        {
            Commands = LoadCommands();
            commandNames = Commands.Select(x => x.Name);
        }

        private static bool ValidateNewCommand(MemeCommand newCmd)
        {
            if (Commands.Contains(newCmd))
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
