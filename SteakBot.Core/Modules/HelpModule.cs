using Discord.Commands;
using SteakBot.Core.Extensions;
using SteakBot.Core.Objects;
using SteakBot.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteakBot.Core.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
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
            var memeCommands = _memeService.MemeCommands;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                memeCommands = memeCommands
                    .Where(x => x.Name.Contains(filter) ||
                                x.Description.Contains(filter))
                    .ToList();
            }

            await ReplyMemeCommands(memeCommands);
        }

        [Command("help")]
        [Summary("Lists all commands with their summaries")]
        public async Task Help()
        {
            await ReplyHelpMessage(_commandService.Commands);
        }

        [Command("help")]
        [Summary("Filters and lists commands with their summaries")]
        [Remarks("Usage: `help <filter(text)>`")]
        public async Task Help(string filter)
        {
            var commands = _commandService.Commands;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                commands = commands
                    .Where(x => x.Name.Contains(filter) ||
                                (x.Remarks != null && x.Remarks.Contains(filter)) ||
                                (x.Summary != null && x.Summary.Contains(filter)));
            }

            await ReplyHelpMessage(commands);
        }

        #region Private Methods

        private async Task ReplyMemeCommands(ICollection<MemeCommand> memeCommands)
        {
            StringBuilder memeListStringBuilder = new StringBuilder();
            foreach (var meme in memeCommands)
            {
                var currMemeFormatted = $"`{meme.Name}` - {meme.Description}";
                if (memeListStringBuilder.Length + currMemeFormatted.Length >= _discordMsgMaxChars)
                {
                    await ReplyAsync(memeListStringBuilder.ToString());
                    memeListStringBuilder.Clear();
                }

                memeListStringBuilder.AppendLine(currMemeFormatted);
            }

            string result = memeListStringBuilder.ToString();
            if (!string.IsNullOrEmpty(result))
            {
                await ReplyAsync(result);
            }
        }

        private async Task ReplyHelpMessage(IEnumerable<CommandInfo> commands)
        {
            // This is set up to use a more streamlined look than previous versions and takes inspiration from the Markdown example at
            // https://gist.github.com/Almeeida/41a664d8d5f3a8855591c2f1e0e07b19
            string commandHelp = string.Join("\n", commands.Select(x => x.CustomToString()));
            if (!string.IsNullOrEmpty(commandHelp))
            {
                string helpMessage = string.Format(HelpMessageFormat, commandHelp);
                await ReplyAsync(helpMessage);
            }
        }

        #endregion Private Methods
    }
}
