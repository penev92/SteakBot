namespace SteakBot.Core.Abstractions.Options
{
    public interface IGitHubIssueNumberOptions
    {
        string IssueIconBaseUrl { get; }

        bool ShouldShowRepositoryIcon { get; }
    }
}
