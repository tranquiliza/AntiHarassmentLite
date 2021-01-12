using AntiHarassmentLite.Core.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AntiHarassmentLite.Core.Handlers
{
    public class SuspensionSaveRequestHandler : IRequestHandler<SaveSuspensionRequest, Unit>
    {
        private readonly ISuspensionRepository suspension;
        private readonly ILogger<SuspensionSaveRequestHandler> logger;

        public SuspensionSaveRequestHandler(ISuspensionRepository suspension, ILogger<SuspensionSaveRequestHandler> logger)
        {
            this.suspension = suspension;
            this.logger = logger;
        }

        public async Task<Unit> Handle(SaveSuspensionRequest request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Saving suspension");

            await suspension.SaveSuspension(request.Suspension).ConfigureAwait(false);

            logger.LogInformation("Suspension saved successfully");
            return Unit.Value;
        }
    }
}
