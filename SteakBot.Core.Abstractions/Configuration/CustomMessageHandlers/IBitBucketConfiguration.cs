namespace SteakBot.Core.Abstractions.Configuration.CustomMessageHandlers
{
    public interface IBitBucketConfiguration
    {
        public string BitBucketIconsBaseUrl { get; }

        public bool ShowRepositoryIcon { get; }
    }
}
