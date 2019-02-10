using System.Threading.Tasks;
using Discord;

namespace SteakBot.Core.EventHandlers.Abstraction
{
	public interface ILogEventHandler
	{
		Task Log(LogMessage msg);
	}
}
