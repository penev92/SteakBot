using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SteakBot.Core.EventHandlers.CustomMessageHandlers.CommandMessageHandlers;
using SteakBot.Core.Objects;
using SteakBot.Core.Objects.Enums;

namespace SteakBot.Core.Modules
{
    public class AddMemeModule : ModuleBase<SocketCommandContext>
    {
        private const char ARG_SEPARATOR = ',';

        [Command("add")]
        public async Task AddMeme([Remainder]string message)
        {
            bool isMessage = true;
            string replyMessage = string.Empty;
            EmbedBuilder embed = null;

            var args = message.Split(ARG_SEPARATOR).ToList();
            if (args.Count != 3)
            {
                replyMessage = "Invalid command format! Refer to help for details.";
            }
            else
            {
                MemeResultType type = args[1].ToLower().Contains("http") ? MemeResultType.Image : MemeResultType.Text;

                var result = CustomCommandMessageHandler.SaveCommand(new MemeCommand(type, args[0], args[1], args[2]));

                isMessage = type == MemeResultType.Text;

                if (result)
                {
                    if (type == MemeResultType.Image)
                    {
                        embed = new EmbedBuilder
                        {
                            ImageUrl = args[1]
                        };
                    }
                    else
                    {
                        replyMessage = args[1];
                    }
                }
                else
                {
                    replyMessage = "Saving failed! ;(";
                }
            }

            await (isMessage
                  ? ReplyAsync(replyMessage)
                  : ReplyAsync(replyMessage, embed:embed.Build()));
        }

        private IEnumerable<IUser> GetUsers()
        {
            var usersAdapter = Context.Channel.GetUsersAsync();
            var users = usersAdapter.ToList().Result.SelectMany(x => x);

            return users;
        }
    }
}
