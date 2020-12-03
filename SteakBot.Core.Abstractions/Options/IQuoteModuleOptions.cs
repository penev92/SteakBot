using System.Collections.Generic;

namespace SteakBot.Core.Abstractions.Options
{
    public interface IQuoteModuleOptions
    {
        IEnumerable<string> TrustedRoles { get; }
    }
}