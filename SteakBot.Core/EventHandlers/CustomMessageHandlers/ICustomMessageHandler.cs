using Discord.WebSocket;

namespace SteakBot.Core.EventHandlers.CustomMessageHandlers
{
	internal interface ICustomMessageHandler
	{
		bool CanHandle(SocketUserMessage message);

		void Invoke(SocketUserMessage message);
	}
}
