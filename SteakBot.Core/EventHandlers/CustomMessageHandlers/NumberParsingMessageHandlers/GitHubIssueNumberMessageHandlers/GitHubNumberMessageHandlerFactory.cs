using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SteakBot.Core.Abstractions.Configuration.CustomMessageHandlers;

namespace SteakBot.Core.EventHandlers.CustomMessageHandlers.NumberParsingMessageHandlers.GitHubIssueNumberMessageHandlers
{
    internal class GitHubNumberMessageHandlerFactory
    {
        private readonly IGitHubConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public GitHubNumberMessageHandlerFactory(IGitHubConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        internal IEnumerable<BaseGitHubIssueNumberMessageHandler> Create()
        {
            return _configuration.Repositories.Values.Select(x => ActivatorUtilities.CreateInstance<BaseGitHubIssueNumberMessageHandler>(_serviceProvider, x));
        }
    }
}
