using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using NAudio.Wave;

namespace SteakBot.Core.Services
{
    public class AudioService
    {
        private readonly ConcurrentDictionary<ulong, List<IAudioClient>> _connectedChannelsPerServer;

        private bool _isPlaying;
        private AudioOutStream _audioOutStream;

        public AudioService(IDiscordClient discordClient)
        {
            var dict = discordClient.GetGuildsAsync().Result.ToDictionary(x => x.Id, y => new List<IAudioChannel>());
            foreach (var kvp in dict)
            {
                _connectedChannelsPerServer.AddOrUpdate(kvp.Key, kvp.Value,
                    (guildId, channels) =>
                    {
                        return _connectedChannelsPerServer[guildId].Union(channels).ToList();
                    });
            }
            _connectedChannelsPerServer = new ConcurrentDictionary<ulong, IList<IAudioClient>>(new Dictionary<ulong, IList<IAudioChannel>>());
            ConcurrentDictionary<int, int> asdf = new ConcurrentDictionary<int, int>(new Dictionary<int, int>());
        }

        public async Task JoinAudioChannel(IGuild guild, IVoiceChannel target)
        {
            if (target.Guild.Id != guild.Id)
            {
                return;
            }

            if (_connectedChannelsPerServer.ContainsKey(guild.Id))
            {
                return;
            }

            var audioClient = await target.ConnectAsync();
            //if (_connectedChannelPerServer.TryAdd(guild.Id, audioClient))
            {
                // If you add a method to log happenings from this service,
                // you can uncomment these commented lines to make use of that.
                //await Log(LogSeverity.Info, $"Connected to voice on {guild.Name}.");
            }
        }

        public async Task LeaveAudioChannel(IGuild guild)
        {
            _isPlaying = false;

            if (_connectedChannelPerServer.TryRemove(guild.Id, out IAudioClient client))
            {
                await client.StopAsync();
                //await Log(LogSeverity.Info, $"Disconnected from voice on {guild.Name}.");
            }
        }

        public async Task PlayAudio(IGuild guild, IMessageChannel sourceTextChannel, string audioFileIdentifier)
        {
            var filePath = ConfigurationManager.AppSettings[audioFileIdentifier];
            if (string.IsNullOrWhiteSpace(filePath))
            {
                await sourceTextChannel.SendMessageAsync("Unknown file identifier.");
                return;
            }

            if (!File.Exists(filePath))
            {
                await sourceTextChannel.SendMessageAsync("File does not exist.");
                return;
            }

            await SendAudioAsync(guild, filePath);
        }

        #region Private methods

        private async Task SendAudioAsync(IGuild guild, string filePath)
        {
            if (_isPlaying)
            {
                return;
            }

            if (_connectedChannelPerServer.TryGetValue(guild.Id, out IAudioClient audioClient))
            {
                _audioOutStream = _audioOutStream ?? audioClient.CreatePCMStream(AudioApplication.Voice);
                using (var mp3 = new Mp3FileReader(filePath))
                using (var pcmStream = WaveFormatConversionStream.CreatePcmStream(mp3))
                {
                    try
                    {
                        _isPlaying = true;
                        await pcmStream.CopyToAsync(_audioOutStream);
                    }
                    finally
                    {
                        await _audioOutStream.FlushAsync();
                        await pcmStream.FlushAsync();
                        _isPlaying = false;
                    }
                }
            }
        }

        #endregion
    }
}
