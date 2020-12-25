using System.Configuration;
using SteakBot.Core.Abstractions.Providers;

namespace SteakBot.Core.Providers
{
    public class AudioFilePathProvider : IAudioFilePathProvider
    {
        public string GetPath(string audioFileIdentifier)
        {
            return ConfigurationManager.AppSettings[audioFileIdentifier];
        }
    }
}
