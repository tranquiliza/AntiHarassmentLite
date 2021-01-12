namespace AntiHarassmentLite.Core.Events
{
    public class JoinChannelRequest
    {
        public string Channel { get; set; }
        public string ChannelToJoin { get; set; }
    }
}
