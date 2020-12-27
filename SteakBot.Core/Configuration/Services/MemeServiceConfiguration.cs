using SteakBot.Core.Abstractions.Configuration.Services;

namespace SteakBot.Core.Configuration.Services
{
    public class MemeServiceConfiguration : BaseConfiguration, IMemeServiceConfiguration
    {
        public string MemeCommandsFileRelativePath { get; set; }
    }
}
