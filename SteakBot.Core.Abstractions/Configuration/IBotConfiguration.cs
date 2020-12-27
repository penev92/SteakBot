using System.Collections.Generic;

namespace SteakBot.Core.Abstractions.Configuration
{
    public interface IBotConfiguration
    {
        string BotToken { get; }

        IEnumerable<string> EnabledModules { get; }
    }
}
