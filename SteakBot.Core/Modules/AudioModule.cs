﻿using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using SteakBot.Core.Services;

namespace SteakBot.Core.Modules
{
    public class AudioModule : ModuleBase<SocketCommandContext>
    {
        private readonly AudioService _service;

        public AudioModule(AudioService service)
        {
            _service = service;
        }

        // You *MUST* mark these commands with 'RunMode.Async'
        // otherwise the bot will not respond until the Task times out.
        [Command("join", RunMode = RunMode.Async)]
        [Summary("Bot joins a chosen voice channel")]
        [Remarks("Usage: `join <voice_channel_name>`")]
        public async Task Join(string channelName)
        {
            if (string.IsNullOrWhiteSpace(channelName))
            {
                await ReplyAsync($"Channel name needed!");
            }

            var channel = Context.Guild.VoiceChannels.FirstOrDefault(x => x.Name == channelName);
            if (channel == null)
            {
                await ReplyAsync($"No such voice channel!");
            }

            await _service.JoinAudioChannel(Context.Guild, channel);
        }

        [Command("leave", RunMode = RunMode.Async)]
        [Summary("Bot leaves the voice channel")]
        public async Task LeaveCmd()
        {
            await _service.LeaveAudioChannel(Context.Guild);
        }

        [Command("gg", RunMode = RunMode.Async)]
        [Summary("Bot says GG in the voice channel")]
        public async Task Gg()
        {
            await _service.PlayAudio(Context.Guild, Context.Channel, "ggVoiceFile");
        }

        [Command("surprise", RunMode = RunMode.Async)]
        public async Task Surprise()
        {
            await _service.PlayAudio(Context.Guild, Context.Channel, "surpriseMf");
        }

        [Command("clownMusic", RunMode = RunMode.Async)]
        public async Task ClownMusic()
        {
            await _service.PlayAudio(Context.Guild, Context.Channel, "clownMusic");
        }
    }
}
