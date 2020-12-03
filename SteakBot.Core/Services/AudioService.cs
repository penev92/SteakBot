using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using NAudio.Wave;
using SteakBot.Core.Abstractions;
using SteakBot.Core.Abstractions.Providers;

namespace SteakBot.Core.Services
{
    public class AudioService
    {
        private readonly IAudioFilePathProvider _audioFilePathProvider;
        private readonly ConcurrentDictionary<ulong, IAudioClient> _connectedChannels = new ConcurrentDictionary<ulong, IAudioClient>();

        private bool _isPlaying;
        private AudioOutStream _audioOutStream;

        public async Task JoinAudioChannel(IGuild guild, IVoiceChannel target)
        {
            if (_connectedChannels.TryGetValue(guild.Id, out _))
            {
                return;
            }
            if (target.Guild.Id != guild.Id)
            {
                return;
            }

            var audioClient = await target.ConnectAsync();

            if (_connectedChannels.TryAdd(guild.Id, audioClient))
            {
                // If you add a method to log happenings from this service,
                // you can uncomment these commented lines to make use of that.
                //await Log(LogSeverity.Info, $"Connected to voice on {guild.Name}.");
            }
        }

        public async Task LeaveAudioChannel(IGuild guild)
        {
            _isPlaying = false;

            if (_connectedChannels.TryRemove(guild.Id, out IAudioClient client))
            {
                await client.StopAsync();
                //await Log(LogSeverity.Info, $"Disconnected from voice on {guild.Name}.");
            }
        }

        public async Task PlayAudio(IGuild guild, IMessageChannel sourceTextChannel, string audioFileIdentifier)
        {
            var filePath = _audioFilePathProvider.GetPath(audioFileIdentifier);
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

            if (_connectedChannels.TryGetValue(guild.Id, out IAudioClient audioClient))
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
