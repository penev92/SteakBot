using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using HtmlAgilityPack;

namespace SteakBot.Core.Modules
{
    class DotaModule : ModuleBase<SocketCommandContext>
    {
        [Command("prizepool")]
        [Summary("Special snowflake command that is only relevant during July and August")]
        public async Task List()
        {
            var uri = "http://www.dota2.com/international/battlepass/";
            var webParser = new HtmlWeb();
            var document = webParser.Load(uri);

            var title = document.DocumentNode.Descendants("title").FirstOrDefault()?.InnerText ?? "Invalid";
            var prize = document.DocumentNode.SelectSingleNode("/html/div/div[3]/div[4]/div[3]/div[2]/div[2]")?.InnerText ?? "Unknown";

            await ReplyAsync($"The current prize pool for `{title}` is: **{prize}**\r\nMore info at {uri}");
        }
    }
}
