using System.Collections.Generic;
using SteakBot.Core.Abstractions.Configuration.CustomMessageHandlers;

namespace SteakBot.Core.Configuration.CustomMessageHandlers
{
    public class BitBucketConfiguration : BaseConfiguration, IBitBucketConfiguration
    {
        public string BitBucketIconsBaseUrl { get; set; }

        public bool ShowRepositoryIcon { get; set; }

        public string ConsumerKey { get; set; }

        public string ConsumerSecretKey { get; set; }

        public IReadOnlyDictionary<string, CodeRepositoryConfiguration> Repositories { get; set; }
    }
}
