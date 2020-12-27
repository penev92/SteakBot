using SteakBot.Core.Abstractions.Configuration.Services;
using SteakBot.Core.Abstractions.Providers;

namespace SteakBot.Core.Providers
{
    public class AudioFilePathProvider : IAudioFilePathProvider
    {
        private readonly IAudioServiceConfiguration _configuration;

        public AudioFilePathProvider(IAudioServiceConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetPath(string audioFileIdentifier)
        {
            return _configuration.AudioFileByIdentifier[audioFileIdentifier];
        }
    }
}
