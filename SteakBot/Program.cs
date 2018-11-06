using SteakBot.Core;

namespace SteakBot
{
	internal class Program
	{
		private static readonly Bot Bot = new Bot();

		private static void Main(string[] args)
		{
			Bot.RunAsync().Wait();
		}
	}
}
