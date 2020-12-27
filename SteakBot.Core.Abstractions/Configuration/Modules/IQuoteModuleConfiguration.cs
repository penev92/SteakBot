using System.Collections.Generic;

namespace SteakBot.Core.Abstractions.Configuration.Modules
{
    public interface IQuoteModuleConfiguration
    {
        IEnumerable<string> TrustedRoles { get; }
    }
}
