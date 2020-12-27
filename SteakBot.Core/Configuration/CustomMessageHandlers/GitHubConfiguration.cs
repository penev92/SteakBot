using SteakBot.Core.Abstractions.Configuration.CustomMessageHandlers;

namespace SteakBot.Core.Configuration.CustomMessageHandlers
{
    public class GitHubConfiguration : BaseConfiguration, IGitHubConfiguration
    {
        public string GitHubIconsBaseUrl { get; set; }

        public bool ShowRepositoryIcon { get; set; }
    }
}
