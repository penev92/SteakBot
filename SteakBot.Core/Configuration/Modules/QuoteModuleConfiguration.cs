using System.Collections.Generic;
using SteakBot.Core.Abstractions.Configuration.Modules;

namespace SteakBot.Core.Configuration.Modules
{
    public class QuoteModuleConfiguration : BaseConfiguration, IQuoteModuleConfiguration
    {
        public IEnumerable<string> TrustedRoles { get; set; }
    }
}
