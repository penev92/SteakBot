using Microsoft.Extensions.Configuration;
using SteakBot.Core.Abstractions.Configuration;
using SteakBot.Core.Abstractions.Configuration.Modules;
using SteakBot.Core.Abstractions.Configuration.Services;
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

        public AppSettingsConfigurationProvider(IConfiguration configuration)
        {
            _configuration = configuration;

            _botConfiguration = new BotConfiguration();

            _audioServiceConfiguration = new AudioServiceConfiguration();
            _memeServiceConfiguration = new MemeServiceConfiguration();
            _quoteModuleConfiguration = new QuoteModuleConfiguration();
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
    }
}
