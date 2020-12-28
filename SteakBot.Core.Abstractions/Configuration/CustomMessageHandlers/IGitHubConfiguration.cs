using System.Collections.Generic;

namespace SteakBot.Core.Abstractions.Configuration.CustomMessageHandlers
{
    public interface IGitHubConfiguration
    {
        string GitHubIconsBaseUrl { get; }
        
        bool ShowRepositoryIcon { get; }

        IReadOnlyDictionary<string, CodeRepositoryConfiguration> Repositories { get; }
    }
}
