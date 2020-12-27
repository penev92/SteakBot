namespace SteakBot.Core.Abstractions.Configuration.CustomMessageHandlers
{
    public interface IGitHubConfiguration
    {
        public string GitHubIconsBaseUrl { get; }

        public bool ShowRepositoryIcon { get; }
    }
}
