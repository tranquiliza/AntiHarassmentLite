using System;
using System.Collections.Generic;
using System.Text;

namespace AntiHarassmentLite.Core.Events
{
    public class UserBannedEvent
    {
        public string Channel { get; set; }
        public string Username { get; set; }
        public DateTime Timestamp { get; set; }
        public string BanReason { get; set; }
    }
}
