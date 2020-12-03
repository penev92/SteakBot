using SteakBot.Core.Abstractions.Options;

namespace SteakBot.Options
{
    public class ConfigurationManagerMemeServiceOptions : IMemeServiceOptions
    {
        public string MemeCommandsFileName { get; }
    }
}
