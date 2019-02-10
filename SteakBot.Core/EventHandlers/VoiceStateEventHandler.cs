using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using SteakBot.Core.EventHandlers.Abstraction;

namespace SteakBot.Core.EventHandlers
{
	internal class VoiceStateEventHandler : IVoiceStateEventHandler
	{
		public async Task HandleUserVoiceStateUpdatedAsync(SocketUser user, SocketVoiceState leaveState, SocketVoiceState joinState)
		{
			if (joinState.VoiceChannel != null)
			{
				// TODO: Perhaps not hardcode this so?
				var channel = joinState.VoiceChannel.Guild.Channels.FirstOrDefault(x => x.Name == "bendoverwatch");
				if (channel is ISocketMessageChannel messageChannel)
				{
					await messageChannel.SendMessageAsync($"{user.Mention} has joined {joinState.VoiceChannel.Name}");
				}
			}
			else
			if (leaveState.VoiceChannel != null)
			{
				// TODO: Perhaps not hardcode this so?
				var channel = leaveState.VoiceChannel.Guild.Channels.FirstOrDefault(x => x.Name == "bendoverwatch");
				if (channel is ISocketMessageChannel messageChannel)
				{
					await messageChannel.SendMessageAsync($"{user.Mention} has ragequit {leaveState.VoiceChannel.Name}");
				}
			}
		}
	}
}
