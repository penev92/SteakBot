using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using SteakBot.Core.Services;

namespace SteakBot.Core.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly MemeService _memeService;
        private readonly CommandService _commandService;

        private readonly int _discordMsgMaxChars = 2000;

        public HelpModule(MemeService memeService, CommandService commandService)
        {
            _memeService = memeService;
            _commandService = commandService;
        }

        [Command("list")]
        [Summary("Lists all available \"memes\"")]
        public async Task List()
        {
            StringBuilder memeListStringBuilder = new StringBuilder();
            foreach(var meme in _memeService.MemeCommands)
            {
                var currMemeFormatted = $"`{meme.Name}` - {meme.Description}";
                if (memeListStringBuilder.Length + currMemeFormatted.Length >= _discordMsgMaxChars)
                {
                    await ReplyAsync(memeListStringBuilder.ToString());
                    memeListStringBuilder.Clear();
                }

                memeListStringBuilder.AppendLine(currMemeFormatted);
            }

            await ReplyAsync(memeListStringBuilder.ToString());
        }

        [Command("help")]
        [Summary("Lists all commands with their summaries")]
        public async Task Help()
        {
            await ReplyAsync($"\n{ string.Join("\n", _commandService.Commands.Select(x => $"**`{x.Name}`** - {x.Summary}")) }\n");
        }
    }
}
