using System.Threading.Tasks;
using Discord.Commands;

namespace SteakBot.Core.Modules
{
    public class ChatModule : ModuleBase<SocketCommandContext>
    {
        [Command("hi")]
        [Summary("Bot says \"Hi\"")]
        public async Task Hi()
        {
            await ReplyAsync($"Hi, {Context.User.Mention} !");
        }

        [Command("say")]
        [Summary("Bot says something")]
        [Remarks("Usage: `say <something>`")]
        public async Task Say([Remainder]string message)
        {
            if (message.EndsWith(GlobalConstants.DeleteMessageChar))
            {
                message = message.Substring(0, message.Length - 1);
            }
            await ReplyAsync(message);
        }
    }
}
