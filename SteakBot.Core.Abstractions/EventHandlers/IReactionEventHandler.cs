using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace SteakBot.Core.Abstractions.EventHandlers
{
    public interface IReactionEventHandler
    {
        Task HandleReactionAddedAsync(Cacheable<IUserMessage, ulong> messageGetter, ISocketMessageChannel channel, SocketReaction reaction);
    }
}