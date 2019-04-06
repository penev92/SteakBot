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
        public async Task AddMeme(string name, string value, string description)
        {
            var replyMessage = string.Empty;
            EmbedBuilder embed = null;

            var type = value.ToLower().Contains("http") ? MemeResultType.Image : MemeResultType.Text;

            var result = _memeService.SaveCommand(new MemeCommand(type, name, value, description));

            var isMessage = type == MemeResultType.Text;

            if (result)
            {
                if (type == MemeResultType.Image)
                {
                    embed = new EmbedBuilder
                    {
                        ImageUrl = value
                    };
                }
                else
                {
                    replyMessage = value;
                }

                await (isMessage
                      ? ReplyAsync(replyMessage)
                      : ReplyAsync(replyMessage, embed: embed.Build()));
            }
            else
            {
                replyMessage = "Saving failed! ;(";
            }
        }
    }
}
