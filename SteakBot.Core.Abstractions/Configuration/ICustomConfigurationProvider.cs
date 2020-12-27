using SteakBot.Core.Abstractions.Configuration.Modules;
using SteakBot.Core.Abstractions.Configuration.Services;

namespace SteakBot.Core.Abstractions.Configuration
{
    public interface ICustomConfigurationProvider
    {
        IBotConfiguration BotConfiguration { get; }

        IQuoteModuleConfiguration QuoteModuleConfiguration { get; }

        IAudioServiceConfiguration AudioServiceConfiguration { get; }

        IMemeServiceConfiguration MemeServiceConfiguration { get; }
    }
}
