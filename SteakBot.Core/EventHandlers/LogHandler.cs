using System;
using System.Threading.Tasks;
using Discord;

namespace SteakBot.Core.EventHandlers
{
	internal static class LogHandler
	{
		internal static Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}
