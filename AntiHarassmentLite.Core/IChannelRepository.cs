using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AntiHarassmentLite.Core
{
    public interface IChannelRepository
    {
        Task<List<string>> GetChannelsToJoin();

        Task JoinChannel(string channelName);
    }
}
