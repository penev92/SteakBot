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

		internal static IList<MemeCommand> Commands { get; } = LoadCommands();

		public CustomCommandMessageHandler()
		{
			CommandNames = Commands.Select(x => x.Name);
		}

		protected override bool InvokeInner(SocketUserMessage message)
		{
			var channel = message.Channel;
			var commandText = message.Content.Replace(CommandChar, "").Replace(DeleteMessageChar, "");
			var command = Commands.Single(x => x.Name == commandText);
			switch (command.MemeResultType)
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

		#endregion
	}
}
