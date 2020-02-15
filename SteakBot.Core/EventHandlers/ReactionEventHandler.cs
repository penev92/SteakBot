using Discord;
using Discord.WebSocket;
using SteakBot.Core.EventHandlers.Abstraction;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SteakBot.Core.EventHandlers
{
    internal class ReactionEventHandler : IReactionEventHandler
    {
        private const int MinReactionMessageHours = 2;
        private const int MessagesAfterCurrentLimit = 10;

        public async Task HandleReactionAddedAsync(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            var channel = arg3.Channel;
            var message = await channel.GetMessageAsync(arg3.MessageId);
            if (await ShouldSkipReaction(channel, message))
            {
                return;
            }

            var messageJumpUrl = message.GetJumpUrl();
            var notificationMessage = $"{arg3.User.Value.Mention} reacted with {arg3.Emote} to [a message]({messageJumpUrl}) by {message.Author.Username}"
                + $" from {message.CreatedAt.LocalDateTime:HH:mm:ss} of {message.CreatedAt.LocalDateTime:dd.MM.yyyy}.";

            var notificationEmbedBuilder = new EmbedBuilder
            {
                Description = notificationMessage
            };

            await channel.SendMessageAsync("", embed: notificationEmbedBuilder.Build());
        }

        private static async Task<bool> ShouldSkipReaction(IMessageChannel channel, IMessage message)
        {
            var messageTimeSpan = DateTime.UtcNow - message.Timestamp.UtcDateTime;
            if (messageTimeSpan.TotalHours < MinReactionMessageHours)
            {
                return true;
            }

            var messages = await channel.GetMessagesAsync(message, Direction.After, MessagesAfterCurrentLimit + 1).ToArray();
            bool shouldSkip = messages.SelectMany(x => x).Count() < MessagesAfterCurrentLimit;
            return shouldSkip;
        }
    }
}
