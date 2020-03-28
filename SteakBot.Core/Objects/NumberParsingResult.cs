namespace SteakBot.Core.Objects
{
    internal class NumberParsingResult
    {
        public string Identifier { get; set; }

        public int Number { get; set; }

        public char? Type { get; set; }

        public NumberParsingResult(string identifier, int number)
        {
            Identifier = identifier;
            Number = number;
        }

        public NumberParsingResult(string identifier, int number, char type)
            : this(identifier, number)
        {
            Type = type;
        }
    }
}
