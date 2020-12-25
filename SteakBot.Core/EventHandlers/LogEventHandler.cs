using System;
using System.Threading.Tasks;
using Discord;
using SteakBot.Core.Abstractions.EventHandlers;

namespace SteakBot.Core.EventHandlers
{
    internal class LogEventHandler : ILogEventHandler
    {
        public Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
