using SteakBot.Core.Objects.Enums;

namespace SteakBot.Core.Objects
{
	internal class MemeCommand
	{
		public MemeResultType MemeResultType { get; set; }

		public string Name { get; set; }

		public string Value { get; set; }

		public string Description { get; set; }

		public MemeCommand(MemeResultType memeResultType, string name, string value, string description)
		{
			MemeResultType = memeResultType;
			Name = name;
			Value = value;
			Description = description;
		}
	}
}
