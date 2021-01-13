using AntiHarassmentLite.Core.Events;
using AntiHarassmentLite.Core.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AntiHarassmentLite.Core
{
    public class BotRunner : IBotRunner
    {
        private readonly IChatClient chatClient;
        private readonly IChannelRepository channelRepository;
        private readonly ILogger<BotRunner> logger;
        private readonly IMediator mediator;

        public BotRunner(IChatClient chatClient, IChannelRepository channelRepository, IMediator mediator, IDateTimeProvider dateTimeProvider, ILogger<BotRunner> logger)
        {
            this.chatClient = chatClient;
            this.channelRepository = channelRepository;
            this.logger = logger;
            this.mediator = mediator;
            this.dateTimeProvider = dateTimeProvider;

            this.chatClient.OnMessageReceived += ChatClient_OnMessageReceived;
            this.chatClient.OnUserTimeoutReceived += ChatClient_OnUserTimeoutReceived;
            this.chatClient.OnUserBannedReceived += ChatClient_OnUserBannedReceived;
            this.chatClient.OnUserLookupReceived += ChatClient_OnUserLookupReceived;
        }

        private async Task ChatClient_OnUserLookupReceived(JoinChannelRequest arg)
        {
            await mediator.Send(new JoinChannelCommandRequest(arg.ChannelToJoin, arg.Channel)).ConfigureAwait(false);
        }

        private string Key(string channel, string username) => $"{channel}:{username}";

        private Task ChatClient_OnUserBannedReceived(UserBannedEvent userBannedEvent)
        {
            var key = Key(userBannedEvent.Channel, userBannedEvent.Username);

            Suspension suspension;

            if (RecentChatMessages.TryGetValue(key, out var chatMessages))
            {
                suspension = Suspension.CreateBan(userBannedEvent, chatMessages);
                logger.LogInformation("Sent save request for: suspension for user: {username}, timestamp: {timestamp} in channel: {channelName}", userBannedEvent.Username, userBannedEvent.Timestamp, userBannedEvent.Channel);
                mediator.Send(new SaveSuspensionRequest(suspension));
            }

            return Task.CompletedTask;
        }

        private Task ChatClient_OnUserTimeoutReceived(UserTimedoutEvent userTimedoutEvent)
        {
            if (userTimedoutEvent.TimeoutDuration < 60)
                return Task.CompletedTask;

            var key = Key(userTimedoutEvent.Channel, userTimedoutEvent.Username);

            Suspension suspension;

            if (RecentChatMessages.TryGetValue(key, out var chatMessages))
            {
                suspension = Suspension.CreateTimeout(userTimedoutEvent, chatMessages);

                logger.LogInformation("Sent save request for: suspension for user: {username}, timestamp: {timestamp} in channel: {channelName}", userTimedoutEvent.Username, userTimedoutEvent.Timestamp, userTimedoutEvent.Channel);
                mediator.Send(new SaveSuspensionRequest(suspension));
            }

            return Task.CompletedTask;
        }

        private readonly Dictionary<string, List<ChatMessageEvent>> RecentChatMessages = new Dictionary<string, List<ChatMessageEvent>>();
        private readonly IDateTimeProvider dateTimeProvider;

        private Task ChatClient_OnMessageReceived(ChatMessageEvent chatMessageEvent)
        {
            var key = Key(chatMessageEvent.Channel, chatMessageEvent.Username);

            if (RecentChatMessages.TryGetValue(key, out var chatMessages))
            {
                var messages = chatMessages.OrderBy(x => x.TimeStamp).ToList();
                messages.RemoveAll(x => x.TimeStamp < dateTimeProvider.UtcNow.Add(TimeSpan.FromMinutes(-10)));

                chatMessages = messages;

                chatMessages.Add(chatMessageEvent);
            }
            else
            {
                RecentChatMessages.Add(key, new List<ChatMessageEvent> { chatMessageEvent });
            }

            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            chatClient.Initialize();
            await chatClient.ConnectAsync().ConfigureAwait(false);
        }

        public async Task JoinChannels()
        {
            foreach (var channelName in await channelRepository.GetChannelsToJoin().ConfigureAwait(false))
                chatClient.JoinChannel(channelName);
        }

        public void Dispose()
        {
            chatClient.Dispose();
        }
    }
}
