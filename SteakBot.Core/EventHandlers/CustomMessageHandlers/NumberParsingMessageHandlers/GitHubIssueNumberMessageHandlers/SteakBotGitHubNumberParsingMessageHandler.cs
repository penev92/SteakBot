using System.Collections.Generic;
using Octokit;
using SteakBot.Core.Abstractions.Configuration.CustomMessageHandlers;

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

        public SteakBotGitHubNumberParsingMessageHandler(IGitHubClient gitHubClient, IGitHubConfiguration configuration) : base(gitHubClient, configuration) { }
    }
}
