using AntiHarassmentLite.Core.Events;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AntiHarassmentLite.Core.Models
{
    public enum SuspensionType
    {
        Timeout = 0,
        Ban = 1
    }

    public class Suspension
    {
        [JsonInclude]
        public UserBannedEvent UserBannedEvent { get; private set; }

        [JsonInclude]
        public UserTimedoutEvent UserTimedoutEvent { get; private set; }

        public string Username
        {
            get
            {
                switch (SuspensionType)
                {
                    case SuspensionType.Timeout:
                        return UserTimedoutEvent.Username;
                    case SuspensionType.Ban:
                        return UserBannedEvent.Username;

                    default:
                        return string.Empty;
                }
            }
        }

        public string Channel
        {
            get
            {
                switch (SuspensionType)
                {
                    case SuspensionType.Timeout:
                        return UserTimedoutEvent.Channel;
                    case SuspensionType.Ban:
                        return UserBannedEvent.Channel;
                    default:
                        return string.Empty;
                }
            }
        }

        public DateTime Timestamp
        {
            get
            {
                switch (SuspensionType)
                {
                    case SuspensionType.Timeout:
                        return UserTimedoutEvent.Timestamp;
                    case SuspensionType.Ban:
                        return UserBannedEvent.Timestamp;
                    default:
                        return DateTime.MinValue;
                }
            }
        }

        [JsonInclude]
        public List<ChatMessageEvent> ChatMessages { get; private set; }

        [JsonInclude]
        public SuspensionType SuspensionType { get; private set; }

        [Obsolete("SerilizationOnly", true)]
        private Suspension() { }

        public Suspension(List<ChatMessageEvent> chatMessages, SuspensionType suspensionType)
        {
            if (chatMessages == null)
                chatMessages = new List<ChatMessageEvent>();

            ChatMessages = chatMessages;
            SuspensionType = suspensionType;
        }

        public static Suspension CreateTimeout(UserTimedoutEvent userTimedoutEvent, List<ChatMessageEvent> chatMessages = null)
            => new Suspension(chatMessages, SuspensionType.Timeout)
            {
                UserTimedoutEvent = userTimedoutEvent
            };

        public static Suspension CreateBan(UserBannedEvent userBannedEvent, List<ChatMessageEvent> chatMessages = null)
            => new Suspension(chatMessages, SuspensionType.Ban)
            {
                UserBannedEvent = userBannedEvent
            };
    }
}
