using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace SteakBot.Core.Modules
{
	public class QuoteModule : ModuleBase<SocketCommandContext>
	{
		[Command("quote")]
		public async Task Quote([Remainder]string message)
		{
			await Context.Channel.DeleteMessagesAsync(new[] { Context.Message }, RequestOptions.Default);

			var users = GetUsers().ToList();
			var userByUsername = users.ToDictionary(x => x.Username, y => y);
			var userByNickname = users.Cast<SocketGuildUser>().Where(x => !string.IsNullOrWhiteSpace(x.Nickname)).ToDictionary(x => x.Nickname, y => y as IUser);
			var userByName = userByNickname.Union(userByUsername).ToDictionary(x => x.Key, y => y.Value);

			var lines = message.Split('\n').ToList();
			SocketTextChannel referredChannel = null;

			foreach (var socketGuildChannel in Context.Guild.Channels)
			{
				var channel = socketGuildChannel as SocketTextChannel;
				if (channel != null && channel.Mention == lines[0].Trim())
				{
					referredChannel = channel;
					lines.RemoveAt(0);
				}
			}

			var embedDescription = new StringBuilder();
			var embedAuthor = new EmbedAuthorBuilder();

			var timestamp = string.Empty;

			foreach (var line in lines)
			{
				var isAuthorLine = false;
				foreach (var user in userByName)
				{
					if (line.StartsWith(user.Key))
					{
						embedAuthor = new EmbedAuthorBuilder
						{
							Name = user.Key,
							IconUrl = user.Value.GetAvatarUrl()
						};

						isAuthorLine = true;

						var trim = line.Substring(user.Key.Length);
						timestamp = trim.Trim();

						break;
					}
				}

				if (!isAuthorLine)
				{
					embedDescription.AppendLine(line);
				}
			}

			var footerText = string.Empty;
			var titleText = string.Empty;

			if (!string.IsNullOrEmpty(timestamp))
			{
				footerText = $"  -  {timestamp}";
				titleText = timestamp;
			}

			if (referredChannel != null)
			{
				footerText += $", in #{referredChannel.Name}";
				titleText += $", in #{referredChannel.Name}";
			}

			var embed = new EmbedBuilder
			{
				Color = Color.Blue,
				//Title = titleText,
				Author = embedAuthor,
				Description = embedDescription.ToString(),
				Footer = new EmbedFooterBuilder { Text = footerText },
			};

			await ReplyAsync("", false, embed);
		}

		private IEnumerable<IUser> GetUsers()
		{
			var usersAdapter = Context.Channel.GetUsersAsync();
			var users = usersAdapter.ToList().Result.SelectMany(x => x);

			return users;
		}
	}
}
