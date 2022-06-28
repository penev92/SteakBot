using Discord;
using Discord.WebSocket;
using SteakBot.Core.Abstractions.EventHandlers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SteakBot.Core.EventHandlers
{
    internal class ReactionEventHandler : IReactionEventHandler
    {
        private const int MinReactionMessageHours = 2;
        private const int MessagesAfterCurrentLimit = 10;

        private static async Task<bool> ShouldSkipReaction(Cacheable<IMessageChannel, ulong> channel, IMessage message)
        {
            var messageTimeSpan = DateTime.UtcNow - message.Timestamp.UtcDateTime;
            if (messageTimeSpan.TotalHours < MinReactionMessageHours)
            {
                return true;
            }

            var messageCount = await channel
                .Value
                .GetMessagesAsync(message, Direction.After, MessagesAfterCurrentLimit + 1)
                .AggregateAsync(0, (i, messagePage) => i + messagePage.Count);

            return messageCount < MessagesAfterCurrentLimit;
        }

        public async Task HandleReactionAddedAsync(Cacheable<IUserMessage, ulong> messageGetter,
            Cacheable<IMessageChannel, ulong> channel,
            SocketReaction reaction)
        {
            var message = await messageGetter.GetOrDownloadAsync();
            if (await ShouldSkipReaction(channel, message))
            {
                return;
            }

            var messageJumpUrl = message.GetJumpUrl();
            var notificationMessage = $"{reaction.User.Value.Mention} reacted with {reaction.Emote} to [a message]({messageJumpUrl}) by {message.Author.Username}"
                                      + $" from {message.CreatedAt.LocalDateTime:HH:mm:ss} of {message.CreatedAt.LocalDateTime:dd.MM.yyyy}.";

            var notificationEmbedBuilder = new EmbedBuilder
            {
                Description = notificationMessage
            };

            await channel.Value.SendMessageAsync("", embed: notificationEmbedBuilder.Build());
        }
    }
}
