using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace SteakBot.Core
{
	internal enum ResultType
	{
		Text = 1,
		Image = 2
	}

	internal class MemeCommand
	{
		public ResultType ResultType { get; set; }

		public string Name { get; set; }

		public string Value { get; set; }

		public string Description { get; set; }

		public MemeCommand(ResultType resultType, string name, string value, string description)
		{
			ResultType = resultType;
			Name = name;
			Value = value;
			Description = description;
		}
	}

	internal class ManualCommandHandler
	{
		internal static List<MemeCommand> Commands = new List<MemeCommand>
		{
			new MemeCommand(ResultType.Text, "shrug", @"¯\_(ツ)_/¯", "ASCII shrug"),
			new MemeCommand(ResultType.Text, "tableFlip", @"(╯°□°）╯︵ ┻━┻", "ASCII table flip"),
			new MemeCommand(ResultType.Text, "tableBack", @"┬──┬◡ﾉ(° -°ﾉ)", "ASCII put the table back"),
			new MemeCommand(ResultType.Image, "balance", @"https://i.imgflip.com/23qudi.jpg", "Spongebob admires the balance"),
			new MemeCommand(ResultType.Image, "magic", @"https://afinde-production.s3.amazonaws.com/uploads/Imagination.png", "Spongebob admires the magic"),
			new MemeCommand(ResultType.Image, "aliens", @"https://imgflip.com/s/meme/Ancient-Aliens.jpg", "Aliens guy with no caption"),
			new MemeCommand(ResultType.Image, "generousGod", @"https://img.memecdn.com/when-i-level-up-someone-amp-039-s-account_o_2942005.jpg", "\"I am a generous God\""),
			new MemeCommand(ResultType.Image, "soGood", @"https://i.kym-cdn.com/entries/icons/original/000/006/077/so_good.png", ""),
			new MemeCommand(ResultType.Image, "justRight", @"https://i.kym-cdn.com/entries/icons/original/000/019/698/d96.jpg", ""),
			new MemeCommand(ResultType.Image, "rabbids", @"https://i.imgur.com/gbkf6qc.mp4", "Raving Rabbids cow chase"),
			new MemeCommand(ResultType.Image, "ifYouKnow", @"https://i.kym-cdn.com/entries/icons/mobile/000/008/549/ifuknowwhatimean.jpg", "\"If you know what I mean\""),
			new MemeCommand(ResultType.Image, "dense", @"https://i.kym-cdn.com/photos/images/newsfeed/000/540/811/e45.png", "\"You dense mother*\""),
			new MemeCommand(ResultType.Image, "strangle", @"http://www.memes.at/faces/must_resist.jpg", ""),
			new MemeCommand(ResultType.Image, "facepalm", @"https://i.kym-cdn.com/entries/icons/mobile/000/000/554/picard-facepalm.jpg", "Captain Picard facepalm"),
			new MemeCommand(ResultType.Image, "doubleFacepalm", @"https://i.kym-cdn.com/photos/images/newsfeed/000/339/529/280.jpg", "Captain Picard + Lieutenant Riker double facepalm"),
			new MemeCommand(ResultType.Image, "notBad", @"https://i.kym-cdn.com/photos/images/newsfeed/000/138/246/tumblr_lltzgnHi5F1qzib3wo1_400.jpg", "Not bad Obama"),
			new MemeCommand(ResultType.Image, "butWhy", @"http://www.reactiongifs.com/r/but-why.gif", "But why, the gif version"),
			new MemeCommand(ResultType.Image, "daamn", @"https://i.imgur.com/G58yDnO.jpg", "DAAAAAMN"),
			new MemeCommand(ResultType.Image, "smart", @"https://www.meme-arsenal.com/memes/ac490d2a8ff94ccdc1f84dc2fb3ba701.jpg", "Smart guy"),
			new MemeCommand(ResultType.Image, "itsRetarded", @"https://i.kym-cdn.com/photos/images/facebook/001/151/981/927.png", "Oh no, it's retarded"),
			new MemeCommand(ResultType.Image, "doubt", @"https://i.imgur.com/uNn17PO.jpg", "Press X for Doubt"),
			new MemeCommand(ResultType.Image, "trap", @"https://i.kym-cdn.com/entries/icons/original/000/000/157/itsatrap.jpg", "Admiral Ackbar \"It's a trap!\""),
			new MemeCommand(ResultType.Image, "like", @"http://static.skaip.org/img/emoticons/180x180/f6fcff/like.gif", "Skype Like emoticon"),
			new MemeCommand(ResultType.Image, "tmi", @"http://static.skaip.org/img/emoticons/180x180/f6fcff/tmi.gif", "Skype TMI emoticon"),
			new MemeCommand(ResultType.Image, "fuckNo", @"https://media.giphy.com/media/12XMGIWtrHBl5e/giphy.gif", "The Office \"NO\" gif"),
			new MemeCommand(ResultType.Image, "blackNo", @"https://media.giphy.com/media/FEikw3bXVHdMk/giphy.gif", "Black \"No way\" gif"),
			new MemeCommand(ResultType.Image, "shrugGif", @"https://www.tenor.co/vxuu.gif", "Muppet shrug gif"),
			new MemeCommand(ResultType.Image, "cwl", @"http://static.skaip.org/img/emoticons/180x180/f6fcff/cwl.gif", "Skype CWL emoticon"),
			new MemeCommand(ResultType.Image, "howAboutNo", @"https://imgflip.com/s/meme/How-About-No-Bear.jpg", "How about no bear"),
			new MemeCommand(ResultType.Image, "nod", @"http://static.skaip.org/img/emoticons/180x180/f6fcff/nod.gif", "Skype nod emoticon"),
			new MemeCommand(ResultType.Image, "grumpyNo", @"https://1u0b5867gsn1ez16a1p2vcj1-wpengine.netdna-ssl.com/wp-content/uploads/2014/09/grumpy-cat-no-1-300x234.jpg", "Grumpy cat \"No\""),
			new MemeCommand(ResultType.Image, "tumbleweed", @"http://static.skaip.org/img/emoticons/180x180/f6fcff/tumbleweed.gif", "Skype tumbleweed emoticon"),
			new MemeCommand(ResultType.Image, "suspicious", @"http://static.skaip.org/img/emoticons/180x180/f6fcff/wonder.gif", "Skype suspicious/wondering emoticon"),
			new MemeCommand(ResultType.Image, "bow", @"http://static.skaip.org/img/emoticons/180x180/f6fcff/bow.gif", "Skype bow emoticon"),
		};

		public static async Task<bool> HandleCommandAsync(SocketUserMessage message)
		{
			var channel = message.Channel;
			var messageString = message.Content;

			if (!messageString.StartsWith("!"))
			{
				return false;
			}

			var commandText = messageString.Substring(1);

			if (messageString.EndsWith("!"))
			{
				commandText = commandText.Substring(0, commandText.Length - 1);
				await channel.DeleteMessagesAsync(new[] { message }, RequestOptions.Default);
			}

			var command = Commands.FirstOrDefault(x => x.Name == commandText);
			if (command == null)
			{
				return false;
			}

			switch (command.ResultType)
			{
				case ResultType.Text:
				{
					await channel.SendMessageAsync(command.Value);
					return true;
				}

				case ResultType.Image:
				{
					var embed = new EmbedBuilder
					{
						ImageUrl = command.Value
					};

					await channel.SendMessageAsync("", embed: embed);
					return true;
				}

				default:
				{
					return false;
				}
			}
		}
	}
}
