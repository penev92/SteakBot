using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using SteakBot.Core.Services;

namespace SteakBot.Core.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly MemeService _memeService;

        public HelpModule(MemeService memeService)
        {
            _memeService = memeService;
        }

        [Command("list")]
        public async Task List()
        {
            await ReplyAsync(string.Join("\r\n", _memeService.MemeCommands.Select(x => $"`{x.Name}` - {x.Description}")));
        }

        [Command("help")]
        public async Task Help()
        {
            await ReplyAsync("`hi` - Bot says \"Hi\"");
            await ReplyAsync("`say <something>` - Bot says something");
            await ReplyAsync("`list` - Lists all available \"memes\"");
            await ReplyAsync("`quote` - Duuh, quotes...");
            await ReplyAsync("`addMeme <commandName> <value(link/ascii art/etc)> <description(use \"\" to format longer descriptions)>` - Add a dank meme of your choosing");
        }
    }
}
