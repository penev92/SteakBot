using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Discord.WebSocket;
using SteakBot.Core.Objects;

namespace SteakBot.Core.EventHandlers.CustomMessageHandlers.NumberParsingMessageHandlers
{
    /// <summary>
    /// Contains the logic for parsing numbers with a leading and trailing identifiers in the format "identifier#123#x".
    /// </summary>
    internal abstract class BaseTypedNumberParsingMessageHandler : BaseNumberParsingMessageHandler
    {
        protected override string RegexMatchPattern { get; } = "(^|[^a-zA-Z0-9])({keyword}#[0-9]+#[a-z])";

        public override bool CanHandle(SocketUserMessage message)
        {
            // This can be simplified when the codebase is migrated to C# 8 / .NET Standard 2.1
            var matchCollectionForPattern = RegexMatchPatterns.Select(regexMatchPattern => Regex.Matches(message.Content, regexMatchPattern, RegexOptions));
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

        protected override IEnumerable<NumberParsingResult> GetMatchedNumbers(string message)
        {
            // This can be simplified when the codebase is migrated to C# 8 / .NET Standard 2.1
            var matchCollectionForPattern = RegexMatchPatterns.Select(regexMatchPattern => Regex.Matches(message, regexMatchPattern, RegexOptions));
            foreach (var matchCollection in matchCollectionForPattern)
            {
                foreach (Match match in matchCollection)
                {
                    var split = match.Groups[match.Groups.Count - 1].Value.Split('#');
                    yield return new NumberParsingResult(split[0], int.Parse(split[1]), split[2][0]);
                }
            }
        }
    }
}
