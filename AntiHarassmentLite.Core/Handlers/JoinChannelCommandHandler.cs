using AntiHarassmentLite.Core.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AntiHarassmentLite.Core.Handlers
{
    public class JoinChannelCommandHandler : IRequestHandler<JoinChannelCommandRequest, Unit>
    {
        private readonly IChatClient chatClient;
        private readonly IChannelRepository channelRepository;

        public JoinChannelCommandHandler(IChatClient chatClient, IChannelRepository channelRepository)
        {
            this.chatClient = chatClient;
            this.channelRepository = channelRepository;
        }

        public async Task<Unit> Handle(JoinChannelCommandRequest request, CancellationToken cancellationToken)
        {
            chatClient.JoinChannel(request.ChannelToJoin);

            await channelRepository.JoinChannel(request.ChannelToJoin).ConfigureAwait(false);

            chatClient.SendMessage(request.ChannelRequesting, $"Successfully joined {request.ChannelToJoin}");

            return Unit.Value;
        }
    }
}
