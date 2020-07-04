using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Discord.WebSocket;

namespace SteakBot.Core.EventHandlers.CustomMessageHandlers.NumberParsingMessageHandlers
{
    internal abstract class BaseNumberParsingMessageHandler : ICustomMessageHandler
    {
        protected abstract Dictionary<string, int> MinimumHandledNumberPerKeyword { get; }

        protected virtual string[] RegexMatchPatternKeywords => MinimumHandledNumberPerKeyword.Keys.ToArray();

        protected virtual string RegexMatchPattern { get; } = "(^|[^a-zA-Z0-9])({keyword}#[0-9]+)";

        protected virtual bool RegexMatchCase { get; } = false;

        private readonly RegexOptions _regexOptions;
        private readonly string[] _regexMatchPatterns;

        internal BaseNumberParsingMessageHandler()
        {
            _regexOptions = RegexMatchCase ? RegexOptions.Compiled : RegexOptions.Compiled | RegexOptions.IgnoreCase;
            _regexMatchPatterns = RegexMatchPatternKeywords.Select(x => RegexMatchPattern.Replace("{keyword}", x)).ToArray();
        }

        public bool CanHandle(SocketUserMessage message)
        {
            // This can be simplified when the codebase is migrated to C# 8 / .NET Standard 2.1
            var matchCollectionForPattern = _regexMatchPatterns.Select(regexMatchPattern => Regex.Matches(message.Content, regexMatchPattern, _regexOptions));
            foreach (var matchCollection in matchCollectionForPattern)
            {
                foreach (Match match in matchCollection)
                {
                    var split = match.Groups[match.Groups.Count - 1].Value.Split('#');
                    var keyword = split[0];
                    if (int.TryParse(split[1], out var number) && MinimumHandledNumberPerKeyword[keyword] <= number)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public abstract void Invoke(SocketUserMessage message);

        protected IEnumerable<int> GetMatchedNumbers(string message)
        {
            // This can be simplified when the codebase is migrated to C# 8 / .NET Standard 2.1
            var matchCollectionForPattern = _regexMatchPatterns.Select(regexMatchPattern => Regex.Matches(message, regexMatchPattern, _regexOptions));
            foreach (var matchCollection in matchCollectionForPattern)
            {
                foreach (Match match in matchCollection)
                {
                    yield return int.Parse(match.Groups[match.Groups.Count - 1].Value.Split('#')[1]);
                }
            }
        }
    }
}
