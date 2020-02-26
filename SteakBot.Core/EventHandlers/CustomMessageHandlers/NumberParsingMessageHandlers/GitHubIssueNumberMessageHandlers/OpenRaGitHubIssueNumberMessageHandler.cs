﻿using System.Collections.Generic;
using Octokit;

namespace SteakBot.Core.EventHandlers.CustomMessageHandlers.NumberParsingMessageHandlers.GitHubIssueNumberMessageHandlers
{
    internal class OpenRaGitHubIssueNumberMessageHandler : BaseGitHubIssueNumberMessageHandler
    {
        protected override string RepositoryOwner { get; } = "OpenRA";

        protected override string RepositoryName { get; } = "OpenRA";

        protected override Dictionary<string, int> MinimumHandledNumberPerKeyword { get; } = new Dictionary<string, int>
        {
            { string.Empty, 2000 },
            { "ora", 0 }
        };

        public OpenRaGitHubIssueNumberMessageHandler(IGitHubClient gitHubClient) : base(gitHubClient) { }
    }
}
