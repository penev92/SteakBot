using System.Collections.Generic;

namespace SteakBot.Core.Abstractions.Configuration.Services
{
    public interface IAudioServiceConfiguration
    {
        IReadOnlyDictionary<string, string> AudioFileByIdentifier { get; }
    }
}
