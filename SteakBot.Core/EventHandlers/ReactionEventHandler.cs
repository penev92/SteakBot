using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using SteakBot.Core.EventHandlers.Abstraction;

namespace SteakBot.Core.EventHandlers
{
    internal class ReactionEventHandler : IReactionEventHandler
    {
        public async Task HandleReactionAddedAsync(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            var channel = arg3.Channel;
            var message = await channel.GetMessageAsync(arg3.MessageId);
            var messageJumpUrl = message.GetJumpUrl();

            var notificationMessage = $"{arg3.User.Value.Mention} reacted with {arg3.Emote} to [a message]({messageJumpUrl}) by {message.Author.Username}"
                + $" from {message.CreatedAt.LocalDateTime:HH:mm:ss} of {message.CreatedAt.LocalDateTime:dd.MM.yyyy}.";

            var notificationEmbedBuilder = new EmbedBuilder
            {
                Description = notificationMessage
            };

            await channel.SendMessageAsync("", embed: notificationEmbedBuilder.Build());
        }
    }
}
