using MediatR;

namespace AntiHarassmentLite.Core.Events
{
    public class JoinChannelCommandRequest : IRequest
    {
        public string ChannelToJoin { get; set; }
        public string ChannelRequesting { get; set; }

        public JoinChannelCommandRequest(string channelToJoin, string channelRequesting)
        {
            ChannelToJoin = channelToJoin;
            ChannelRequesting = channelRequesting;
        }
    }
}
