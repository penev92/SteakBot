using System.Collections.Generic;

namespace SteakBot.Core.Abstractions.Configuration.CustomMessageHandlers
{
    public class CodeRepositoryConfiguration
    {
        public string IconsBaseUrl { get; set; }

        public bool ShowRepositoryIcon { get; set; }

        public string Owner { get; set; }
        
        public string Name { get; set; }

        public Dictionary<string, int> MinimumHandledNumberPerKeyword { get; set; }
    }
}
