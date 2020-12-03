using System.Threading.Tasks;
using Discord;

namespace SteakBot.Core.Abstractions.Handlers
{
    public interface ILogEventHandler
    {
        Task Log(LogMessage msg);
    }
}
