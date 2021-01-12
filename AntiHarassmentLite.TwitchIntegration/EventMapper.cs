using AntiHarassmentLite.Core;
using AntiHarassmentLite.Core.Events;
using TwitchLib.Client.Models;

namespace AntiHarassmentLite.TwitchIntegration
{
    public static class EventMapper
    {
        public static ChatMessageEvent MapToChatMessageEvent(this ChatMessage chatMessage, IDateTimeProvider dateTimeProvider)
        {
            return new ChatMessageEvent
            {
                TimeStamp = dateTimeProvider.UtcNow,
                ColorHex = chatMessage.ColorHex,
                Username = chatMessage.Username,
                DisplayName = chatMessage.DisplayName,
                IsHighlighted = chatMessage.IsHighlighted,
                IsVip = chatMessage.IsVip,
                IsSubscriber = chatMessage.IsSubscriber,
                IsSkippingSubMode = chatMessage.IsSkippingSubMode,
                IsModerator = chatMessage.IsModerator,
                IsMe = chatMessage.IsMe,
                IsBroadcaster = chatMessage.IsBroadcaster,
                SubscribedMonthCount = chatMessage.SubscribedMonthCount,
                Channel = chatMessage.Channel,
                Message = chatMessage.Message
            };
        }

        public static UserTimedoutEvent MapToTimedOutEvent(this UserTimeout userTimeout, IDateTimeProvider dateTimeProvider)
        {
            return new UserTimedoutEvent
            {
                Channel = userTimeout.Channel,
                TimeoutDuration = userTimeout.TimeoutDuration,
                TimeoutReason = userTimeout.TimeoutReason,
                Timestamp = dateTimeProvider.UtcNow,
                Username = userTimeout.Username
            };
        }

        public static UserBannedEvent MapToBannedEvent(this UserBan userBan, IDateTimeProvider dateTimeProvider)
        {
            return new UserBannedEvent
            {
                BanReason = userBan.BanReason,
                Channel = userBan.Channel,
                Timestamp = dateTimeProvider.UtcNow,
                Username = userBan.Username
            };
        }
    }
}
