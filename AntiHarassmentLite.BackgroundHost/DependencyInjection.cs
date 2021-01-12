using AntiHarassmentLite.Core;
using AntiHarassmentLite.Sql;
using AntiHarassmentLite.TwitchIntegration;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AntiHarassmentLite.BackgroundHost
{
    public static class DependencyInjection
    {
        public static void ConfigureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetRequiredValue<string>("ConnectionStrings:SqlDatabase");
            services.AddLogging(builder =>
            {
                var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
                if (string.Equals(environmentName, "development", StringComparison.OrdinalIgnoreCase))
                    builder.AddConsole();
                else
                    builder.AddSeq(configuration.GetSection("Seq"));
            });

            var twitchChatSettings = new TwitchClientSettings(configuration.GetRequiredValue<string>("Twitch:Username"), configuration.GetRequiredValue<string>("Twitch:OAuth"));
            services.AddSingleton(twitchChatSettings);

            services.AddSingleton<IBotRunner, BotRunner>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddMediatR(typeof(DependencyInjection).GetTypeInfo().Assembly, typeof(IBotRunner).GetTypeInfo().Assembly);

            services.AddSingleton<IChatClient, TwitchChatClient>();
            services.AddSingleton<ISuspensionRepository, SuspensionRepository>(x => new SuspensionRepository(connectionString, x.GetRequiredService<ILogger<SuspensionRepository>>()));
            services.AddSingleton<IChannelRepository, ChannelRepository>(x => new ChannelRepository(connectionString, x.GetRequiredService<ILogger<ChannelRepository>>()));

            services.AddHostedService<BotService>();
        }
    }
}
