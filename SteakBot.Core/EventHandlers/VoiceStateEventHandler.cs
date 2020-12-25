using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SteakBot.Core.Abstractions.EventHandlers;
using Discord.WebSocket;

namespace SteakBot.Core.EventHandlers
{
    internal class VoiceStateEventHandler : IVoiceStateEventHandler
    {
        // TODO: Get from configuration.
        private const string FallbackMessageChannel = "bendoverwatch";

        // TODO: Get from configuration.
        private readonly IReadOnlyDictionary<string, string> _voiceToMessageChannelMapping = new Dictionary<string, string>
        {
            { "shavilandia", "bendoverwatch" },
            { "starcraft_voice", "zurg_op" },
            { "random_voice", "parjoli" },
            { "play_diablo_here_pliz", "general_gaming" }
        };

        public async Task HandleUserVoiceStateUpdatedAsync(SocketUser user, SocketVoiceState oldState, SocketVoiceState newState)
        {
            // Join
            if (oldState.VoiceChannel == null && newState.VoiceChannel != null)
            {
                await SendMessageAsync(newState, $"{user.Mention} has joined {newState.VoiceChannel.Name}");
            }
            // Leave
            else if (oldState.VoiceChannel != null && newState.VoiceChannel == null)
            {
                await SendMessageAsync(oldState, $"{user.Mention} has ragequit {oldState.VoiceChannel.Name}");
            }
            // Change
            else if (oldState.VoiceChannel != null && newState.VoiceChannel != null
                     && oldState.VoiceChannel.Name != newState.VoiceChannel.Name)
            {
                await SendMessageAsync(newState, $"{user.Mention} has moved from {oldState.VoiceChannel.Name} to {newState.VoiceChannel.Name}");
            }
        }

        private async Task SendMessageAsync(SocketVoiceState state, string message)
        {
            var messageChannelName = GetMessageChannelForVoiceEvent(state.VoiceChannel.Name);
            var channel = state.VoiceChannel.Guild.Channels.FirstOrDefault(x => x.Name == messageChannelName);
            if (channel is ISocketMessageChannel messageChannel)
            {
                await messageChannel.SendMessageAsync(message);
            }
        }

        private string GetMessageChannelForVoiceEvent(string voiceChannelName)
        {
            if (_voiceToMessageChannelMapping.ContainsKey(voiceChannelName))
            {
                return _voiceToMessageChannelMapping[voiceChannelName];
            }

            // Fallback
            return FallbackMessageChannel;
        }
    }
}
