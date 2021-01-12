using System;
using System.Collections.Generic;
using System.Text;

namespace AntiHarassmentLite.Core.Events
{
    public class UserTimedoutEvent
    {
        public DateTime Timestamp { get; set; }
        public string Channel { get; set; }
        public int TimeoutDuration { get; set; }
        public string TimeoutReason { get; set; }
        public string Username { get; set; }
    }
}
