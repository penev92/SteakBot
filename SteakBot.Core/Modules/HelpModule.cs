using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using SteakBot.Core.Extensions;
using SteakBot.Core.Modules.Quotes;
using SteakBot.Core.Objects;
using SteakBot.Core.Services;

namespace SteakBot.Core.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        // This is set up to use a more streamlined look than previous versions and takes inspiration from the Markdown example at
        // https://gist.github.com/Almeeida/41a664d8d5f3a8855591c2f1e0e07b19
        private const string HelpMessageFormat = "```md\n{0}\n```";

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
            await ReplyMemeCommands(_memeService.MemeCommands);
        }

        [Command("list")]
        [Summary("Filters and lists available \"memes\"")]
        [Remarks("Usage: `list <filter(text)>`")]
        public async Task List(string filter)
        {
            var filterToLower = filter.ToLower();
            var memeCommands = _memeService.MemeCommands;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                memeCommands = memeCommands
                    .Where(x => x.Name.ToLower().Contains(filterToLower) ||
                                x.Description.ToLower().Contains(filterToLower))
                    .ToList();
            }

            await ReplyMemeCommands(memeCommands);
        }

        [Command("help")]
        [Summary("Lists all commands with their summaries")]
        public async Task Help()
        {
            await ReplyHelpMessage(_commandService.Commands.ToArray());
        }

        [Command("help")]
        [Summary("Filters and lists commands with their summaries")]
        [Remarks("Usage: `help <filter(text)>`")]
        public async Task Help(string filter)
        {
            var filterToLower = filter.ToLower();

            var commands = _commandService.Commands;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                commands = commands
                    .Where(x => x.Name.ToLower().Contains(filterToLower) ||
                                (x.Remarks != null && x.Remarks.ToLower().Contains(filterToLower)) ||
                                (x.Summary != null && x.Summary.ToLower().Contains(filterToLower)));
            }

            await ReplyHelpMessage(commands.ToArray());
        }

        #region Private Methods

        private async Task ReplyMemeCommands(IEnumerable<MemeCommand> memeCommands)
        {
            var memeListStringBuilder = new StringBuilder();
            foreach (var meme in memeCommands)
            {
                var currentMemeFormatted = $"`{meme.Name}` - {meme.Description}";
                if (memeListStringBuilder.Length + currentMemeFormatted.Length >= _discordMsgMaxChars)
                {
                    await ReplyAsync(memeListStringBuilder.ToString());
                    memeListStringBuilder.Clear();
                }

                memeListStringBuilder.AppendLine(currentMemeFormatted);
            }

            var result = memeListStringBuilder.ToString();
            if (!string.IsNullOrEmpty(result))
            {
                await ReplyAsync(result);
            }
        }

        private async Task ReplyHelpMessage(ICollection<CommandInfo> commands)
        {
            foreach (var block in GenerateHelpBlocks(commands))
            {
                if (!string.IsNullOrWhiteSpace(block))
                {
                    await ReplyAsync(string.Format(HelpMessageFormat, block));
                }
            }
        }

        private IEnumerable<string> GenerateHelpBlocks(ICollection<CommandInfo> commands)
        {
            yield return string.Join("\n", commands.Where(x => x.Module.Name != nameof(QuoteModule)).Select(x => x.CustomToString()));
            yield return string.Join("\n", commands.Where(x => x.Module.Name == nameof(QuoteModule)).Select(x => x.CustomToString()));
        }

        #endregion
    }
}
