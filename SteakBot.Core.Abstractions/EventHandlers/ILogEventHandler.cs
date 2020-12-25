using System.Threading.Tasks;
using Discord;

namespace SteakBot.Core.Abstractions.EventHandlers
{
    public interface ILogEventHandler
    {
        Task Log(LogMessage msg);
    }
}
