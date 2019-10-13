using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SteakBot.Core.Objects;
using SteakBot.Core.Objects.Enums;
using SteakBot.Core.Services;

namespace SteakBot.Core.Modules
{
    public class MemeModule : ModuleBase<SocketCommandContext>
    {
        private readonly MemeService _memeService;

        public MemeModule(MemeService memeService)
        {
            _memeService = memeService;
        }

        [Command("addMeme")]
        [Summary("`addMeme <commandName> <value(text/link)> <description(use \"\" when you have spaces)>` - Add a dank meme of your choosing")]
        public async Task AddMeme(string name, string value, string description)
        {
            var type = _memeService.GetMemeType(value);

            var result = _memeService.AddCommand(new MemeCommand(type, name, value, description));

            await SendResponse(result, type, value, "Add failed! ;(");
        }

        [Command("editMeme")]
        [Summary("`editMem <commandName> <newCommandName> <newValue(text/link)> <newDescription(use \"\" when you have spaces)>` - Evolve a meme of your choosing")]
        public async Task EditMeme(string oldName, string newName, string newValue, string newDescription)
        {
            var type = _memeService.GetMemeType(newValue);

            var result = _memeService.EditCommand(oldName, new MemeCommand(type, newName, newValue, newDescription));

            await SendResponse(result, type, newValue, "Edit failed! ;(");
        }

        [Command("removeMeme")]
        [Summary("`removeMeme <commandName>` - Remove an ancient meme")]
        public async Task RemoveMeme(string name)
        {
            var result = _memeService.RemoveCommand(name);

            var replyMessage = result ? "Removed" : "Remove failed! ;(";

            await ReplyAsync(replyMessage);
        }

        #region Private methods

        private Task SendResponse(bool commandResult, MemeResultType memeType, string value, string failureMessage)
        {
            var replyMessage = string.Empty;
            var embed = new EmbedBuilder();

            if (commandResult)
            {
                if (memeType == MemeResultType.Image)
                {
                    embed.ImageUrl = value;
                }
                else
                {
                    replyMessage = value;
                }
            }
            else
            {
                replyMessage = failureMessage;
            }
            return ReplyAsync(replyMessage, embed: embed?.Build());
        }

        #endregion
    }
}
