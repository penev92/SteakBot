using System.Collections.Generic;

namespace SteakBot.Core.Abstractions.Configuration.CustomMessageHandlers
{
    public interface IBitBucketConfiguration
    {
        string BitBucketIconsBaseUrl { get; }

        bool ShowRepositoryIcon { get; }

        string ConsumerKey { get; }

        string ConsumerSecretKey { get; }

        IReadOnlyDictionary<string, CodeRepositoryConfiguration> Repositories { get; }
    }
}
