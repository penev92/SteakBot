using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using SteakBot.Core.Objects;
using SteakBot.Core.Objects.Enums;

namespace SteakBot.Core.EventHandlers.CustomMessageHandlers.CommandMessageHandlers
{
	internal class ManualCommandHandler : ICustomMessageHandler
	{
		private const string CommandChar = "!";
		private const string DeleteMessageChar = "!";

		private static readonly string MemeCommandsFileName = ConfigurationManager.AppSettings["memeCommandsRelativeFilePath"];
		private static readonly Lazy<IList<MemeCommand>> CommandsLazy = new Lazy<IList<MemeCommand>>(LoadCommands);
		
		internal static IList<MemeCommand> Commands => CommandsLazy.Value;

		public bool CanHandle(SocketUserMessage message)
		{
			var messageString = message.Content;
			var commandText = messageString.Replace(CommandChar, "").Replace(DeleteMessageChar, "");
			return messageString.StartsWith(CommandChar) && Commands.Any(x => x.Name == commandText);
		}

		public async void Invoke(SocketUserMessage message)
		{
			var channel = message.Channel;

			if (ShouldDeleteMessage(message))
			{
				await channel.DeleteMessageAsync(message, RequestOptions.Default);
			}

			var commandText = message.Content.Replace(CommandChar, "").Replace(DeleteMessageChar, "");
			var command = Commands.Single(x => x.Name == commandText);
			switch (command.MemeResultType)
			{
				case MemeResultType.Text:
				{
					await channel.SendMessageAsync(command.Value);
					return;
				}

				case MemeResultType.Image:
				{
					var embed = new EmbedBuilder
					{
						ImageUrl = command.Value
					};

					await channel.SendMessageAsync("", embed: embed.Build());
					return;
				}

				default:
				{
					return;
				}
			}
		}

		#region Private methods

		private static bool ShouldDeleteMessage(SocketUserMessage message)
		{
			return message.Content.EndsWith(DeleteMessageChar);
		}

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
