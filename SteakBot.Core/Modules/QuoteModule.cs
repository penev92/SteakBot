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
        [Summary("Duuh, quotes...")]
        public async Task Quote([Remainder]string message)
        {
            await Context.Channel.DeleteMessageAsync(Context.Message, RequestOptions.Default);

            var users = GetUsers().ToList();
            var usersByUsername = users.GroupBy(x => x.Username);
            var usersByNickname = users.Cast<SocketGuildUser>().Where(x => !string.IsNullOrWhiteSpace(x.Nickname)).GroupBy(x => x.Nickname);
            var usersByNameTmp = usersByNickname.Union(usersByUsername).GroupBy(x => x.Key);
            var usersByName = usersByNameTmp.ToDictionary(x => x.Key, y => y.SelectMany(z => z).ToList());

            var lines = message.Split('\n').ToList();
            SocketTextChannel referredChannel = null;

            foreach (var socketGuildChannel in Context.Guild.Channels)
            {
                if (socketGuildChannel is SocketTextChannel channel && channel.Mention == lines[0].Trim())
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
                foreach (var userList in usersByName)
                {
                    if (line.StartsWith(userList.Key))
                    {
                        embedAuthor = new EmbedAuthorBuilder
                        {
                            Name = userList.Key,
                            IconUrl = userList.Value.Count == 1 ? userList.Value.First().GetAvatarUrl() : null
                        };

                        isAuthorLine = true;

                        var trim = line.Substring(userList.Key.Length);
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

            if (!string.IsNullOrEmpty(timestamp))
            {
                footerText = $"  -  {timestamp}";
            }

            if (referredChannel != null)
            {
                footerText += $", in #{referredChannel.Name}";
            }

            var embed = new EmbedBuilder
            {
                Color = Color.Blue,
                Author = embedAuthor,
                Description = embedDescription.ToString(),
                Footer = new EmbedFooterBuilder { Text = footerText }
            };

            await ReplyAsync("", false, embed.Build());
        }

        private IEnumerable<IUser> GetUsers()
        {
            var usersAdapter = Context.Channel.GetUsersAsync();
            var users = usersAdapter.ToList().Result.SelectMany(x => x);

            return users;
        }
    }
}
