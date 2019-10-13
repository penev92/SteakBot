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

        public HelpModule(MemeService memeService, CommandService commandService)
        {
            _memeService = memeService;
            _commandService = commandService;
        }

        [Command("list")]
        [Summary("Lists all available \"memes\"")]
        public async Task List()
        {
            await ReplyAsync(string.Join("\r\n", _memeService.MemeCommands.Select(x => $"`{x.Name}` - {x.Description}")));
        }

        [Command("help")]
        [Summary("Lists all commands with their summaries")]
        public async Task Help()
        {
            await ReplyAsync($"\n{ string.Join("\n", _commandService.Commands.Select(x => $"**`{x.Name}`** - {x.Summary}")) }\n");
        }
    }
}
