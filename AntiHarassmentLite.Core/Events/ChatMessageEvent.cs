using System;
using System.Collections.Generic;
using System.Text;

namespace AntiHarassmentLite.Core.Events
{
    public class ChatMessageEvent
    {
        public DateTime TimeStamp { get; set; }
        public string ColorHex { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public bool IsHighlighted { get; set; }
        public bool IsVip { get; set; }
        public bool IsSubscriber { get; set; }
        public bool IsSkippingSubMode { get; set; }
        public bool IsModerator { get; set; }
        public bool IsMe { get; set; }
        public bool IsBroadcaster { get; set; }
        public int SubscribedMonthCount { get; set; }
        public string Channel { get; set; }
        public string Message { get; set; }
    }
}
