using AntiHarassmentLite.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AntiHarassmentLite.Core
{
    public interface IChatClient : IDisposable
    {
        event Func<ChatMessageEvent, Task> OnMessageReceived;
        event Func<UserTimedoutEvent, Task> OnUserTimeoutReceived;
        event Func<UserBannedEvent, Task> OnUserBannedReceived;
        event Func<JoinChannelRequest, Task> OnUserLookupReceived;

        void Initialize();
        Task ConnectAsync();
        void JoinChannel(string channelName);
        void LeaveChannel(string channelName);
        void SendMessage(string channel, string message);
    }
}
