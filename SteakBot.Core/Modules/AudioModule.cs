using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.Commands;
using NAudio.Wave;

namespace SteakBot.Core.Modules
{
	public class AudioModule : ModuleBase<SocketCommandContext>
	{
		// Scroll down further for the AudioService.
		// Like, way down
		private readonly AudioService _service;

		// Remember to add an instance of the AudioService
		// to your IServiceCollection when you initialize your bot
		public AudioModule(AudioService service)
		{
			_service = service;
		}

		// You *MUST* mark these commands with 'RunMode.Async'
		// otherwise the bot will not respond until the Task times out.
		[Command("join", RunMode = RunMode.Async)]
		public async Task Join()
		{
			await _service.JoinAudio(Context.Guild, Context.Guild.VoiceChannels.First());
		}

		// Remember to add preconditions to your commands,
		// this is merely the minimal amount necessary.
		// Adding more commands of your own is also encouraged.
		[Command("leave", RunMode = RunMode.Async)]
		public async Task LeaveCmd()
		{
			await _service.LeaveAudio(Context.Guild);
		}

		[Command("gg", RunMode = RunMode.Async)]
		public async Task Gg()
		{
			await _service.SendAudioAsync(Context.Guild, Context.Channel, @"C:\Users\Pavel\Desktop\gg.mp3");
		}
	}

	public class AudioService
	{
		private readonly ConcurrentDictionary<ulong, IAudioClient> _connectedChannels = new ConcurrentDictionary<ulong, IAudioClient>();
		private AudioOutStream _audioOutStream;
		private bool _isPlaying;

		public async Task JoinAudio(IGuild guild, IVoiceChannel target)
		{
			IAudioClient client;
			if (_connectedChannels.TryGetValue(guild.Id, out client))
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

			IAudioClient client;
			if (_connectedChannels.TryRemove(guild.Id, out client))
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

			IAudioClient audioClient;
			if (_connectedChannels.TryGetValue(guild.Id, out audioClient))
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


	//class Asdf
	//{
	//	[Command("join", RunMode = RunMode.Async)]
	//	public async Task Join()
	//	{
	//		var audioClient = await Context.Guild.VoiceChannels.First().ConnectAsync();
	//	}

	//	[Command("gg", RunMode = RunMode.Async)]
	//	public async Task Gg()
	//	{
	//		//using (var audioStream = audioChannel.CreatePCMStream(AudioApplication.Voice))
	//		//using (var mp3 = new Mp3FileReader(@"C:\Users\Pavel\Desktop\gg.mp3"))
	//		//using (var pcmStream = WaveFormatConversionStream.CreatePcmStream(mp3))
	//		//{
	//		//	//WaveFileWriter.CreateWaveFile(_outPath_, pcm);
	//		//	//try { await ffmpeg.StandardOutput.BaseStream.CopyToAsync(audioStream); }
	//		//	try
	//		//	{
	//		//		await pcmStream.CopyToAsync(audioStream);
	//		//	}
	//		//	finally
	//		//	{
	//		//		await audioStream.FlushAsync();
	//		//		await pcmStream.FlushAsync();
	//		//	}
	//		//}
	//	}

	//	[Command("leave", RunMode = RunMode.Async)]
	//	public async Task Leave()
	//	{

	//		//await audioChannel.StopAsync();
	//	}
	//}
}
