using System.Collections.Generic;
using SteakBot.Core.Abstractions.EventHandlers;

namespace SteakBot.Core.Abstractions.Providers
{
    public interface ICustomMessageHandlerProvider
    {
        IEnumerable<ICustomMessageHandler> Get();
    }
}
