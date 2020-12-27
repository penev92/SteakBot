using System.Collections.Generic;
using SteakBot.Core.Abstractions.Configuration.Services;

namespace SteakBot.Core.Configuration.Services
{
    public class AudioServiceConfiguration : BaseConfiguration, IAudioServiceConfiguration
    {
        public IReadOnlyDictionary<string, string> AudioFileByIdentifier { get; set; }
    }
}
