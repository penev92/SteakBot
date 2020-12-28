using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SteakBot.Core.Abstractions.Configuration.CustomMessageHandlers;

namespace SteakBot.Core.EventHandlers.CustomMessageHandlers.NumberParsingMessageHandlers.BitBucketIssueNumberMessageHandlers
{
    internal class BitBucketNumberMessageHandlerFactory
    {
        private readonly IBitBucketConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public BitBucketNumberMessageHandlerFactory(IBitBucketConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        internal IEnumerable<BaseBitBucketIssueNumberMessageHandler> Create()
        {
            return _configuration.Repositories.Values.Select(x => ActivatorUtilities.CreateInstance<BaseBitBucketIssueNumberMessageHandler>(_serviceProvider, x));
        }
    }
}
