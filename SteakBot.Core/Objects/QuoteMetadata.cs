using Discord;

namespace SteakBot.Core.Objects
{
    public class QuoteMetadata
    {
        public IUser Author { get; }

        public string AuthorName { get; }

        public string Timestamp { get; }

        public QuoteMetadata(IUser author, string authorName, string timestamp)
        {
            Author = author;
            AuthorName = authorName;
            Timestamp = timestamp;
        }
    }
}
