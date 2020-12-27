using SteakBot.Core.Abstractions.Configuration.CustomMessageHandlers;

namespace SteakBot.Core.Configuration.CustomMessageHandlers
{
    public class BitBucketConfiguration : BaseConfiguration, IBitBucketConfiguration
    {
        public string BitBucketIconsBaseUrl { get; set; }

        public bool ShowRepositoryIcon { get; set; }
    }
}
