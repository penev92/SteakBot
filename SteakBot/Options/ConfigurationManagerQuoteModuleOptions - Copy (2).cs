using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using SteakBot.Core.Abstractions;
using SteakBot.Core.Abstractions.Options;
using SteakBot.Core.Modules.Quotes;

namespace SteakBot.Options
{
    public class ConfigurationManagerQuoteModuleOptions : IQuoteModuleOptions
    {
        public IEnumerable<string> TrustedRoles => ConfigurationManager
            .AppSettings["TrustedRoles"]
            .Split(';')
            .Select(x => x.Trim())
            .ToArray();
    }
}
