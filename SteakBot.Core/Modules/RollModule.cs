using System.Threading.Tasks;
using Discord.Commands;

namespace SteakBot.Core.Modules
{
    public class RollModule : ModuleBase<SocketCommandContext>
    {
        [Command("roll")]
        [Summary("Roll a random number between 1 and 100")]
        public async Task Roll()
        {
            var rolledNumber = ThreadLocalRandom.Instance.Next(1, 100);

            await ReplyAsync($"{Context.User.Mention} rolled {rolledNumber} (1-100).");
        }

        [Command("roll")]
        [Summary("Roll a random number between `min` and `max`")]
        [Remarks("Usage: `roll min max`.")]
        public async Task Roll(int min, int max)
        {
            int rolledNumber;
            if (min == max)
            {
                rolledNumber = min;
            }
            else if (min > max)
            {
                rolledNumber = 0;
            }
            else
            {
                rolledNumber = ThreadLocalRandom.Instance.Next(min, max);
            }

            await ReplyAsync($"{Context.User.Mention} rolled {rolledNumber} ({min}-{max}).");
        }

        [Command("iq")]
        [Summary("Shows your IQ")]
        public async Task Iq()
        {
            var rolledIq = ThreadLocalRandom.Instance.Next(-1, 180);

            await ReplyAsync($"{Context.User.Mention} has an IQ of {rolledIq}.");
        }

        [Command("sr")]
        [Summary("Shows your SR")]
        public async Task Sr()
        {
            var rolledSr = ThreadLocalRandom.Instance.Next(0, 5000);

            await ReplyAsync($"{Context.User.Mention} has an SR of {rolledSr}.");
        }
    }
}
