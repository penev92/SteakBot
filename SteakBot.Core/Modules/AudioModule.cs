using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using SteakBot.Core.Services;

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

            await _service.JoinAudio(Context.Guild, channel);
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
}
