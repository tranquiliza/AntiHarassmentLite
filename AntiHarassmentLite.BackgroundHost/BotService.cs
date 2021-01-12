using AntiHarassmentLite.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AntiHarassmentLite.BackgroundHost
{
    public class BotService : IHostedService
    {
        private readonly IBotRunner botRunner;
        private readonly ILogger<BotService> logger;

        public BotService(IBotRunner botRunner, ILogger<BotService> logger)
        {
            this.botRunner = botRunner;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting Robot");

            await botRunner.InitializeAsync().ConfigureAwait(true);
            await botRunner.JoinChannels().ConfigureAwait(true);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Stopping Robot");
            botRunner.Dispose();

            return Task.CompletedTask;
        }
    }
}
