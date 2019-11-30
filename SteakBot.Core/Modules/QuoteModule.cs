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
            await Context.Channel.DeleteMessageAsync(Context.Message);

            var embed = CreateEmbed(message);
            await ReplyAsync("", false, embed);
        }

        [Command("quote")]
        [Summary("Duuh, quotes...")]
        public async Task Quote(string channelMention, ulong messageId)
        {
            if (!TryGetTextChannelByMention(channelMention, out var channel))
            {
                await ReplyAsync("Unknown channel specified.");
                return;
            }

            await Context.Channel.DeleteMessageAsync(Context.Message, RequestOptions.Default);

            var message = await channel.GetMessageAsync(messageId);
            var embed = CreateEmbed(message);
            await ReplyAsync("", false, embed);
        }

        #region Private methods

        private static IEnumerable<IUser> GetChannelUsers(IChannel channel)
        {
            var usersAdapter = channel.GetUsersAsync();
            var users = usersAdapter.ToList().Result.SelectMany(x => x);

            return users;
        }

        private static Dictionary<string, List<IUser>> GetChannelUsersDictionary(IChannel channel)
        {
            var users = GetChannelUsers(channel).ToList();
            var usersByUsername = users.GroupBy(x => x.Username);
            var usersByNickname = users.Cast<SocketGuildUser>().Where(x => !string.IsNullOrWhiteSpace(x.Nickname)).GroupBy(x => x.Nickname);
            var usersByNameTmp = usersByNickname.Union(usersByUsername).GroupBy(x => x.Key);
            var usersByName = usersByNameTmp.ToDictionary(x => x.Key, y => y.SelectMany(z => z).ToList());

            return usersByName;
        }

        private bool TryGetTextChannelByMention(string channelMention, out SocketTextChannel channel)
        {
            foreach (var socketGuildChannel in Context.Guild.Channels)
            {
                if (socketGuildChannel is SocketTextChannel tmpChannel && tmpChannel.Mention == channelMention)
                {
                    channel = tmpChannel;
                    return true;
                }
            }

            channel = null;
            return false;
        }

        private static bool TryGetQuoteAuthorAndTimestamp(string messageLine, IChannel channel, out IUser author, out string authorName, out string timestamp)
        {
            var usersByName = GetChannelUsersDictionary(channel);

            foreach (var userList in usersByName)
            {
                if (messageLine.StartsWith(userList.Key))
                {
                    author = userList.Value.Count == 1 ? userList.Value.First() : null;
                    authorName = userList.Key;

                    var trim = messageLine.Substring(userList.Key.Length);
                    timestamp = trim.Trim();

                    return true;
                }
            }

            author = null;
            authorName = null;
            timestamp = null;
            return false;
        }

        private static EmbedAuthorBuilder BuildAuthorEmbed(IUser author, string authorNameToUse)
        {
            if (author == null)
            {
                return null;
            }

            return new EmbedAuthorBuilder
            {
                Name = authorNameToUse,
                IconUrl = author.GetAvatarUrl()
            };
        }

        private static EmbedFooterBuilder BuildFooterEmbed(IChannel referredChannel, string timestamp)
        {
            var footerText = string.Empty;

            if (!string.IsNullOrEmpty(timestamp))
            {
                footerText = $"  -  {timestamp}";
            }

            if (referredChannel != null)
            {
                footerText += $", in #{referredChannel.Name}";
            }

            return new EmbedFooterBuilder { Text = footerText };
        }

        private Embed CreateEmbed(string message)
        {
            var lines = message.Split('\n').ToList();

            if (TryGetTextChannelByMention(lines[0].Trim(), out var referredChannel))
            {
                lines.RemoveAt(0);
            }

            if (TryGetQuoteAuthorAndTimestamp(lines[0].Trim(), Context.Channel, out var author, out var authorName, out var timestamp))
            {
                lines.RemoveAt(0);
            }

            var embed = new EmbedBuilder
            {
                Color = Color.Blue,
                Author = BuildAuthorEmbed(author, authorName),
                Description = string.Join("\n", lines),
                Footer = BuildFooterEmbed(referredChannel, timestamp)
            };

            return embed.Build();
        }

        private static Embed CreateEmbed(IMessage message)
        {
            var author = message.Author;
            var authorName = (author as SocketGuildUser)?.Nickname ?? author.Username;
            var referredChannel = message.Channel;
            var timestamp = message.Timestamp.ToString();

            var embed = new EmbedBuilder
            {
                Color = Color.Blue,
                Author = BuildAuthorEmbed(author, authorName),
                Description = message.Content,
                Footer = BuildFooterEmbed(referredChannel, timestamp)
            };

            return embed.Build();
        }

        #endregion
    }
}
