using System.Threading.Tasks;
using Discord.Commands;

namespace SteakBot.Core.Modules
{
    public class MemeModule : ModuleBase<SocketCommandContext>
    {
        [Command("hi")]
        public async Task Hi()
        {
            await ReplyAsync($"Hi, {Context.User.Mention} !");
        }

        [Command("say")]
        public async Task Say([Remainder]string message)
        {
            await ReplyAsync(message);
        }
    }
}
