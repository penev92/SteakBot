using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using SteakBot.Core.Abstractions.Configuration;
using SteakBot.Core.Abstractions.Configuration.CustomMessageHandlers;
using SteakBot.Core.Abstractions.Configuration.Modules;
using SteakBot.Core.Abstractions.Configuration.Services;
using SteakBot.Core.Configuration.CustomMessageHandlers;
using SteakBot.Core.Configuration.Modules;
using SteakBot.Core.Configuration.Services;

namespace SteakBot.Core.Configuration
{
    public class AppSettingsConfigurationProvider : ICustomConfigurationProvider
    {
        private readonly IConfiguration _configuration;

        private readonly BotConfiguration _botConfiguration;

        private readonly AudioServiceConfiguration _audioServiceConfiguration;
        private readonly MemeServiceConfiguration _memeServiceConfiguration;
        private readonly QuoteModuleConfiguration _quoteModuleConfiguration;
        private readonly BitBucketConfiguration _bitBucketConfiguration;
        private readonly GitHubConfiguration _gitHubConfiguration;

        public AppSettingsConfigurationProvider(IConfiguration configuration)
        {
            _configuration = configuration;

            _botConfiguration = new BotConfiguration();

            _audioServiceConfiguration = new AudioServiceConfiguration();
            _memeServiceConfiguration = new MemeServiceConfiguration();
            _quoteModuleConfiguration = new QuoteModuleConfiguration();
            _bitBucketConfiguration = new BitBucketConfiguration();
            _gitHubConfiguration = new GitHubConfiguration();
        }

        public IBotConfiguration BotConfiguration
        {
            get
            {
                if (!_botConfiguration.IsPopulated)
                {
                    _botConfiguration.IsPopulated = true;
                    _configuration.GetSection("Bot").Bind(_botConfiguration);
                }

                return _botConfiguration;
            }
        }

        public IQuoteModuleConfiguration QuoteModuleConfiguration
        {
            get
            {
                if (!_quoteModuleConfiguration.IsPopulated)
                {
                    _quoteModuleConfiguration.IsPopulated = true;
                    _configuration.GetSection("StandardCommandModules:QuoteModule").Bind(_quoteModuleConfiguration);
                }

                return _quoteModuleConfiguration;
            }
        }

        public IAudioServiceConfiguration AudioServiceConfiguration
        {
            get
            {
                if (!_audioServiceConfiguration.IsPopulated)
                {
                    _audioServiceConfiguration.IsPopulated = true;
                    _configuration.GetSection("Services:AudioService").Bind(_audioServiceConfiguration);
                }

                return _audioServiceConfiguration;
            }
        }

        public IMemeServiceConfiguration MemeServiceConfiguration
        {
            get
            {
                if (!_memeServiceConfiguration.IsPopulated)
                {
                    _memeServiceConfiguration.IsPopulated = true;
                    _configuration.GetSection("Services:MemeService").Bind(_memeServiceConfiguration);
                }

                return _memeServiceConfiguration;
            }
        }

        public IBitBucketConfiguration BitBucketConfiguration
        {
            get
            {
                if (!_bitBucketConfiguration.IsPopulated)
                {
                    _bitBucketConfiguration.IsPopulated = true;
                    _configuration.GetSection("CustomMessageHandlers:NumberParsingMessageHandlers:BitBucket").Bind(_bitBucketConfiguration);
                }

                return _bitBucketConfiguration;
            }
        }

        public IGitHubConfiguration GitHubConfiguration
        {
            get
            {
                if (!_gitHubConfiguration.IsPopulated)
                {
                    _gitHubConfiguration.IsPopulated = true;
                    _configuration.GetSection("CustomMessageHandlers:NumberParsingMessageHandlers:GitHub").Bind(_gitHubConfiguration);

                    foreach (var (_, value) in _gitHubConfiguration.Repositories)
                    {
                        value.IconsBaseUrl = _gitHubConfiguration.GitHubIconsBaseUrl;
                        value.ShowRepositoryIcon = _gitHubConfiguration.ShowRepositoryIcon;

                        if (value.MinimumHandledNumberPerKeyword == null)
                        {
                            value.MinimumHandledNumberPerKeyword = new Dictionary<string, int>();
                        }
                    }
                }

                return _gitHubConfiguration;
            }
        }
    }
}
