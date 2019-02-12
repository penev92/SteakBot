using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using NAudio.Wave;

namespace SteakBot.Core.Modules
{
    public class AudioService
    {
        private readonly ConcurrentDictionary<ulong, IAudioClient> _connectedChannels = new ConcurrentDictionary<ulong, IAudioClient>();
        private AudioOutStream _audioOutStream;
        private bool _isPlaying;

        public async Task JoinAudio(IGuild guild, IVoiceChannel target)
        {
            if (_connectedChannels.TryGetValue(guild.Id, out IAudioClient client))
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

        public async Task LeaveAudio(IGuild guild)
        {
            _isPlaying = false;

            if (_connectedChannels.TryRemove(guild.Id, out IAudioClient client))
            {
                await client.StopAsync();
                //await Log(LogSeverity.Info, $"Disconnected from voice on {guild.Name}.");
            }
        }

        public async Task SendAudioAsync(IGuild guild, IMessageChannel channel, string filePath)
        {
            if (_isPlaying)
            {
                return;
            }

            // Your task: Get a full path to the file if the value of 'path' is only a filename.
            if (!File.Exists(filePath))
            {
                await channel.SendMessageAsync("File does not exist.");
                return;
            }

            if (_connectedChannels.TryGetValue(guild.Id, out IAudioClient audioClient))
            {
                //using (var audioStream = audioClient.CreatePCMStream(AudioApplication.Voice))
                _audioOutStream = _audioOutStream ?? audioClient.CreatePCMStream(AudioApplication.Voice);
                using (var mp3 = new Mp3FileReader(filePath))
                using (var pcmStream = WaveFormatConversionStream.CreatePcmStream(mp3))
                {
                    //WaveFileWriter.CreateWaveFile(_outPath_, pcm);
                    //try { await ffmpeg.StandardOutput.BaseStream.CopyToAsync(audioStream); }
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
                //using (var ffmpeg = CreateProcess(path))
                //using (var stream = audioClient.CreatePCMStream(AudioApplication.Music))
                //{
                //	try { await ffmpeg.StandardOutput.BaseStream.CopyToAsync(stream); }
                //	finally { await stream.FlushAsync(); }
                //}
            }
        }

        private Process CreateProcess(string filePath)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $"-hide_banner -loglevel panic -i \"{filePath}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
        }
    }
}
