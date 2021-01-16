using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SteakBot.Core.Abstractions.EventHandlers;
using SteakBot.Core.Abstractions.Providers;
using SteakBot.Core.EventHandlers.CustomMessageHandlers.NumberParsingMessageHandlers.BitBucketIssueNumberMessageHandlers;
using SteakBot.Core.EventHandlers.CustomMessageHandlers.NumberParsingMessageHandlers.GitHubIssueNumberMessageHandlers;

namespace SteakBot.Core.Providers
{
    public class CustomMessageHandlerProvider : ICustomMessageHandlerProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomMessageHandlerProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<ICustomMessageHandler> Get()
        {
            var handlers = _serviceProvider.GetServices<ICustomMessageHandler>();
            foreach (var customMessageHandler in handlers)
            {
                yield return customMessageHandler;
            }

            var gitHubHandlers = _serviceProvider.GetServices<BaseGitHubIssueNumberMessageHandler>();
            foreach (var customMessageHandler in gitHubHandlers)
            {
                yield return customMessageHandler;
            }

            var bitBucketHandlers = _serviceProvider.GetServices<BaseBitBucketIssueNumberMessageHandler>();
            foreach (var customMessageHandler in bitBucketHandlers)
            {
                yield return customMessageHandler;
            }
        }
    }
}
