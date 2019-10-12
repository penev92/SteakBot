using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using NAudio.Wave;

namespace SteakBot.Core.Services
{
    public class AudioTuple : IDisposable
    {
        public IAudioClient AudioClient;

        public IVoiceChannel VoiceChannel;

        public AudioOutStream AudioStream;

        public bool IsPlaying;

        public AudioTuple(IAudioClient audioClient, IVoiceChannel voiceChannel, AudioOutStream audioStream)
        {
            AudioClient = audioClient;
            VoiceChannel = voiceChannel;
            AudioStream = audioStream;
            IsPlaying = false;
        }

        public void Dispose()
        {
            IsPlaying = false;
            AudioStream?.Dispose();
            AudioStream = null;
            AudioClient?.StopAsync();
            //AudioClient?.Dispose();
        }
    }

    public class AudioService : IDisposable
    {
        private readonly ConcurrentDictionary<ulong, AudioTuple> _audioSetupPerServer = new ConcurrentDictionary<ulong, AudioTuple>();

        public async Task JoinAudioChannel(IGuild guild, IVoiceChannel targetVoiceChannel)
        {
            if (targetVoiceChannel.Guild.Id != guild.Id)
            {
                return;
            }

            if (_audioSetupPerServer.ContainsKey(guild.Id))
            {
                return;
            }

            var audioClient = await targetVoiceChannel.ConnectAsync();
            var audioStream = audioClient.CreatePCMStream(AudioApplication.Voice);
            _audioSetupPerServer.TryAdd(guild.Id, new AudioTuple(audioClient, targetVoiceChannel, audioStream));
        }

        public async Task LeaveAudioChannel(IGuild guild)
        {
            if (_audioSetupPerServer.TryRemove(guild.Id, out var audioTuple))
            {
                audioTuple.Dispose();
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
            if (!_audioSetupPerServer.TryGetValue(guild.Id, out var audioTuple))
            {
                return;
            }

            if (audioTuple.IsPlaying)
            {
                return;
            }

            using (var mp3 = new Mp3FileReader(filePath))
            using (var pcmStream = WaveFormatConversionStream.CreatePcmStream(mp3))
            {
                try
                {
                    audioTuple.IsPlaying = true;
                    await pcmStream.CopyToAsync(audioTuple.AudioStream);
                }
                finally
                {
                    await audioTuple.AudioStream.FlushAsync();
                    await pcmStream.FlushAsync();
                    audioTuple.IsPlaying = false;
                }
            }
        }

        #endregion

        public void Dispose()
        {
            foreach (var kvp in _audioSetupPerServer)
            {
                kvp.Value.Dispose();
            }
        }
    }
}
