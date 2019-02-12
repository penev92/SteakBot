using SteakBot.Core.Objects.Enums;

namespace SteakBot.Core.Objects
{
    internal class MemeCommand
    {
        public MemeResultType ResultType { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public MemeCommand(MemeResultType resultType, string name, string value, string description)
        {
            ResultType = resultType;
            Name = name;
            Value = value;
            Description = description;
        }
    }
}
