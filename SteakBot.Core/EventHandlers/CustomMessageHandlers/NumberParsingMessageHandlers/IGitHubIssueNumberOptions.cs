using System;
using System.Collections.Generic;
using System.Text;

namespace SteakBot.Core.EventHandlers.CustomMessageHandlers.NumberParsingMessageHandlers
{
    public interface IGitHubIssueNumberOptions
    {
        string IssueIconBaseUrl { get; }

        bool ShouldShowRepositoryIcon { get; }
    }
}
