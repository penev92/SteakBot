namespace SteakBot.Core.Abstractions.Options
{
    public interface IBitBucketIssueNumberMessageHandlerOptions
    {
        string IssueIconBaseUrl { get; }

        bool ShouldShowRepositoryIcon { get; }
    }
}
