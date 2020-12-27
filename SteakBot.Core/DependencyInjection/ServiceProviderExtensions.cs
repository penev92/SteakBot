using System.IO;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Octokit;
using SharpBucket.V2;
using SteakBot.Core.Abstractions;
using SteakBot.Core.Abstractions.Configuration;
using SteakBot.Core.EventHandlers;
using SteakBot.Core.Abstractions.EventHandlers;
using SteakBot.Core.Abstractions.Providers;
using SteakBot.Core.Configuration;
using SteakBot.Core.EventHandlers.CustomMessageHandlers;
using SteakBot.Core.EventHandlers.CustomMessageHandlers.CommandMessageHandlers;
using SteakBot.Core.EventHandlers.CustomMessageHandlers.NumberParsingMessageHandlers.GitHubIssueNumberMessageHandlers;
using SteakBot.Core.Modules;
using SteakBot.Core.Providers;
using SteakBot.Core.Services;
using SteakBot.Core.TypeReaders;

namespace SteakBot.Core.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static IServiceCollection AddAppSettingsConfiguration(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IConfiguration>(provider =>
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json");

                    return builder.Build();
                })
                .AddSingleton<ICustomConfigurationProvider, AppSettingsConfigurationProvider>();
        }

        public static IServiceCollection AddBasicDiscordServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>();
        }

        public static IServiceCollection AddDefaultDiscordBot(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IBot, Bot>()
                .AddSingleton(serviceProvider =>
                {
                    var configurationProvider = serviceProvider.GetService<ICustomConfigurationProvider>();
                    return configurationProvider.BotConfiguration;
                });
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
                .AddSingleton<ModuleBase<SocketCommandContext>, RollModule>()
                .AddSingleton<AudioService>()
                .AddSingleton<MemeService>()
                .AddSingleton<QuotingService>()
                .AddSingleton<IAudioFilePathProvider, AudioFilePathProvider>()
                .AddSingleton(serviceProvider =>
                {
                    var configurationProvider = serviceProvider.GetService<ICustomConfigurationProvider>();
                    return configurationProvider.QuoteModuleConfiguration;
                })
                .AddSingleton(serviceProvider =>
                {
                    var configurationProvider = serviceProvider.GetService<ICustomConfigurationProvider>();
                    return configurationProvider.AudioServiceConfiguration;
                })
                .AddSingleton(serviceProvider =>
                {
                    var configurationProvider = serviceProvider.GetService<ICustomConfigurationProvider>();
                    return configurationProvider.MemeServiceConfiguration;
                });
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
                .AddSingleton(serviceProvider =>
                {
                    var configurationProvider = serviceProvider.GetService<ICustomConfigurationProvider>();
                    return configurationProvider.GitHubConfiguration;
                })
                .AddSingleton<IGitHubClient>(provider => new GitHubClient(new ProductHeaderValue("SteakBot")))
                .AddSingleton<ICustomMessageHandler, SteakBotGitHubNumberParsingMessageHandler>()
                .AddSingleton<ICustomMessageHandler, OpenRaGitHubIssueNumberMessageHandler>();
        }

        public static IServiceCollection AddBitBucketIntegrationServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton(serviceProvider =>
                {
                    var configurationProvider = serviceProvider.GetService<ICustomConfigurationProvider>();
                    return configurationProvider.BitBucketConfiguration;
                })
                .AddSingleton<SharpBucketV2>();
        }
    }
}
