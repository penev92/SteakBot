using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Octokit;
using SteakBot.Core.EventHandlers;
using SteakBot.Core.EventHandlers.Abstraction;
using SteakBot.Core.EventHandlers.CustomMessageHandlers;
using SteakBot.Core.EventHandlers.CustomMessageHandlers.CommandMessageHandlers;
using SteakBot.Core.EventHandlers.CustomMessageHandlers.NumberParsingMessageHandlers.GitHubIssueNumberMessageHandlers;
using SteakBot.Core.Modules;
using SteakBot.Core.Services;
using SteakBot.Core.TypeReaders;

namespace SteakBot.Core.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static IServiceCollection AddBasicDiscordServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>();
        }

        public static IServiceCollection AddDefaultEventHandlerServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ILogEventHandler, LogEventHandler>()
                .AddSingleton<IMessageEventHandler, MessageEventHandler>()
                .AddSingleton<IReactionEventHandler, ReactionEventHandler>()
                .AddSingleton<IVoiceStateEventHandler, VoiceStateEventHandler>();
        }

        public static IServiceCollection AddCustomTypeReaders(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<BaseTypeReader, UriTypeReader>()
                .AddSingleton<BaseTypeReader, DiscordMessageIdentifierTypeReader>();
        }

        public static IServiceCollection AddDefaultModules(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ModuleBase<SocketCommandContext>, AudioModule>()
                .AddSingleton<ModuleBase<SocketCommandContext>, DotaModule>()
                .AddSingleton<ModuleBase<SocketCommandContext>, HelpModule>()
                .AddSingleton<ModuleBase<SocketCommandContext>, ChatModule>()
                .AddSingleton<ModuleBase<SocketCommandContext>, QuoteModule>()
                .AddSingleton<ModuleBase<SocketCommandContext>, MemeModule>()
                .AddSingleton<AudioService>()
                .AddSingleton<MemeService>()
                .AddSingleton<QuotingService>();
        }

        public static IServiceCollection AddDefaultCustomMessageHandlers(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<ICustomMessageHandler, CustomCommandMessageHandler>()
                .AddSingleton<ICustomMessageHandler, StandardCommandMessageHandler>();
        }

        public static IServiceCollection AddGitHubIntegrationServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IGitHubClient>(provider => new GitHubClient(new ProductHeaderValue("SteakBot")))
                .AddSingleton<ICustomMessageHandler, SteakBotGitHubNumberParsingMessageHandler>();
        }
    }
}
