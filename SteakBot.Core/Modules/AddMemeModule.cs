using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SteakBot.Core.EventHandlers.CustomMessageHandlers;
using SteakBot.Core.EventHandlers.CustomMessageHandlers.CommandMessageHandlers;
using SteakBot.Core.Objects;
using SteakBot.Core.Objects.Enums;

namespace SteakBot.Core.Modules
{
    public class AddMemeModule : ModuleBase<SocketCommandContext>
    {
        private readonly CustomCommandMessageHandler _customCommandMessageHandler;

        public AddMemeModule(IEnumerable<ICustomMessageHandler> customCommandMessageHandler)
        {
            _customCommandMessageHandler = (CustomCommandMessageHandler)customCommandMessageHandler.First(x => x is CustomCommandMessageHandler);
        }

        [Command("addMeme")]
        public async Task AddMeme(string name, string value, string description)
        {
            var isMessage = true;
            var replyMessage = string.Empty;
            EmbedBuilder embed = null;

            MemeResultType type = value.ToLower().Contains("http") ? MemeResultType.Image : MemeResultType.Text;

            var result = _customCommandMessageHandler.SaveCommand(new MemeCommand(type, name, value, description));

            isMessage = type == MemeResultType.Text;

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
            }
            else
            {
                replyMessage = "Saving failed! ;(";
            }

            await (isMessage
                  ? ReplyAsync(replyMessage)
                  : ReplyAsync(replyMessage, embed:embed.Build()));
        }
    }
}
