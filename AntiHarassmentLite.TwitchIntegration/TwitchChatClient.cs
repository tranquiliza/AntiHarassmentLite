using AntiHarassmentLite.Core;
using AntiHarassmentLite.Core.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace AntiHarassmentLite.TwitchIntegration
{
    public class TwitchChatClient : IChatClient
    {
        private readonly TwitchClientSettings twitchClientSettings;
        private readonly ILogger<TwitchChatClient> logger;
        private readonly TwitchClient client;

        public event Func<ChatMessageEvent, Task> OnMessageReceived;
        public event Func<UserTimedoutEvent, Task> OnUserTimeoutReceived;
        public event Func<UserBannedEvent, Task> OnUserBannedReceived;
        public event Func<JoinChannelRequest, Task> OnUserLookupReceived;

        public TwitchChatClient(TwitchClientSettings twitchClientSettings, ILogger<TwitchChatClient> logger, IDateTimeProvider dateTimeProvider)
        {
            this.twitchClientSettings = twitchClientSettings;
            this.logger = logger;
            client = new TwitchClient();
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnUserTimedout += Client_OnUserTimedout;
            client.OnUserBanned += Client_OnUserBanned;
            client.OnChatCommandReceived += Client_OnChatCommandReceived;
            this.dateTimeProvider = dateTimeProvider;
        }

        private void Client_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            if (!string.Equals(e.Command.CommandText, "join", StringComparison.OrdinalIgnoreCase))
                return;

            if (e.Command.ArgumentsAsList.Count < 1)
                return;

            var isAllowedToExecute = string.Equals(e.Command.ChatMessage.Username, "tranquiliza", StringComparison.OrdinalIgnoreCase);
            if (!isAllowedToExecute)
                return;

            var firstArgument = e.Command.ArgumentsAsList[0];

            var commandEvent = new JoinChannelRequest
            {
                ChannelToJoin = firstArgument,
                Channel = e.Command.ChatMessage.Channel
            };

            OnUserLookupReceived?.Invoke(commandEvent);
        }

        public void SendMessage(string channel, string message)
        {
            client.SendMessage(channel, message);
        }

        private void Client_OnUserBanned(object sender, OnUserBannedArgs e)
        {
            logger.LogInformation("User banned: {username} from channel {channel}", e.UserBan.Username, e.UserBan.Channel);
            OnUserBannedReceived?.Invoke(e.UserBan.MapToBannedEvent(dateTimeProvider));
        }

        private void Client_OnUserTimedout(object sender, OnUserTimedoutArgs e)
        {
            logger.LogInformation("User timedout: {username} from channel {channel}", e.UserTimeout.Username, e.UserTimeout.Channel);
            OnUserTimeoutReceived?.Invoke(e.UserTimeout.MapToTimedOutEvent(dateTimeProvider));
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            OnMessageReceived?.Invoke(e.ChatMessage.MapToChatMessageEvent(dateTimeProvider));
        }

        public void Initialize()
        {
            var credentials = new ConnectionCredentials(twitchClientSettings.TwitchUsername, twitchClientSettings.TwitchBotOAuth);
            client.Initialize(credentials, chatCommandIdentifier: '?');
            logger.LogInformation("Twitch chat client initialized");
        }

        public void JoinChannel(string channelName)
        {
            var clientHasAlreadyJoined = client.JoinedChannels.Any(x => string.Equals(x.Channel, channelName, StringComparison.OrdinalIgnoreCase));
            if (clientHasAlreadyJoined)
                return;

            client.JoinChannel(channelName);

            logger.LogInformation($"Joined channel: {channelName}");
        }

        public void LeaveChannel(string channelName)
        {
            client.LeaveChannel(channelName);

            logger.LogInformation($"Leaving channel: {channelName}");
        }

        //public void SendMessage(string channel, string message)
        //{
        //    if (!client.JoinedChannels.Any(x => string.Equals(x.Channel, channel, StringComparison.OrdinalIgnoreCase)))
        //    {
        //        logger.LogWarning("Was unable to send message to {arg}'s channel, the client is no longer connected?", channel);
        //        return;
        //    }

        //    client.SendMessage(channel, message);
        //}

        private TaskCompletionSource<bool> connectionCompletionTask = new TaskCompletionSource<bool>();
        private readonly IDateTimeProvider dateTimeProvider;

        public async Task ConnectAsync()
        {
            client.OnConnected += TwitchClient_OnConnected;
            client.Connect();

            await connectionCompletionTask.Task.ConfigureAwait(false);
            logger.LogInformation("Twitch chat client connected");
        }

        private void TwitchClient_OnConnected(object sender, OnConnectedArgs e)
        {
            client.OnConnected -= TwitchClient_OnConnected;

            connectionCompletionTask.SetResult(true);
            connectionCompletionTask = new TaskCompletionSource<bool>();
        }

        public void Dispose()
        {
            client.Disconnect();
            logger.LogInformation("Twitch client disconnect sent");
        }
    }
}
