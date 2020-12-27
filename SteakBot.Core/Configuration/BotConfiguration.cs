using System.Collections.Generic;
using SteakBot.Core.Abstractions.Configuration;

namespace SteakBot.Core.Configuration
{
    public class BotConfiguration : BaseConfiguration, IBotConfiguration
    {
        public string BotToken { get; set; }

        public IEnumerable<string> EnabledModules { get; set; }
    }
}
