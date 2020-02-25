using System.Collections.Generic;

namespace SteakBot.Core.EventHandlers.CustomMessageHandlers.NumberParsingMessageHandlers.GitHubIssueNumberMessageHandlers
{
    internal class SteakBotGitHubNumberParsingMessageHandler : BaseGitHubIssueNumberMessageHandler
    {
        protected override string RepositoryOwner { get; } = "penev92";

        protected override string RepositoryName { get; } = "SteakBot";

        protected override Dictionary<string, int> MinimumHandledNumberPerKeyword { get; } = new Dictionary<string, int>
        {
            { "bot", 0 }
        };
    }
}
