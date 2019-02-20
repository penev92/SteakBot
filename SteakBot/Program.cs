namespace SteakBot
{
    internal class Program
    {
        private static void Main()
        {
            using (var startup = new Startup())
            {
                startup.ConfigureServices();
                var botHostBuilder = new BotHostBuilder().UseServiceProvider(startup.ServiceProvider);
                var botHost = botHostBuilder.Build();
                botHost.RunAsync().Wait();
            }
        }
    }
}
