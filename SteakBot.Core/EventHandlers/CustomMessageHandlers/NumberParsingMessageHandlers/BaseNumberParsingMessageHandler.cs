using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Discord.WebSocket;
using SteakBot.Core.Objects;

namespace SteakBot.Core.EventHandlers.CustomMessageHandlers.NumberParsingMessageHandlers
{
    /// <summary>
    /// Contains the configuration for number parsing.
    /// </summary>
    internal abstract class BaseNumberParsingMessageHandler : ICustomMessageHandler
    {
        protected abstract Dictionary<string, int> MinimumHandledNumberPerKeyword { get; }

        protected virtual string[] RegexMatchPatternKeywords => MinimumHandledNumberPerKeyword.Keys.ToArray();

        protected abstract string RegexMatchPattern { get; }

        protected virtual bool RegexMatchCase { get; } = false;

        protected readonly RegexOptions RegexOptions;
        protected readonly string[] RegexMatchPatterns;

        internal BaseNumberParsingMessageHandler()
        {
            RegexOptions = RegexMatchCase ? RegexOptions.Compiled : RegexOptions.Compiled | RegexOptions.IgnoreCase;
            RegexMatchPatterns = RegexMatchPatternKeywords.Select(x => RegexMatchPattern.Replace("{keyword}", x)).ToArray();
        }

        public abstract bool CanHandle(SocketUserMessage message);

        public abstract void Invoke(SocketUserMessage message);

        protected abstract IEnumerable<NumberParsingResult> GetMatchedNumbers(string message);
    }
}
