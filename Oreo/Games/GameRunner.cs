namespace Oreo.Games
{
    using IO.Ably;
    using IO.Ably.Realtime;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using static Oreo.Games.Constants;

    public class GameRunner : IGameRunner
    {
        public IGame Game { get; }

        private readonly IChannels<IRealtimeChannel> channels;
        private readonly List<IRealtimeChannel> activeChannels;

        public GameRunner(
            IGame game,
            IChannels<IRealtimeChannel> channels)
        {
            this.Game = game;
            this.channels = channels;
            this.activeChannels = new List<IRealtimeChannel>();
        }

        public string GetPlayerChannelId()
        {
            var channelId = Guid.NewGuid().ToString();
            var channel = this.channels.Get(channelId);
            channel.Subscribe(this.HandleMessage);
            return channelId;
        }

        private void HandleMessage(Message message)
        {
            var playerAction = JsonConvert.DeserializeObject<PlayerAction>(message.Data.ToString());

            if (playerAction.ActionName == PlayerActionName.AddMe)
            {
                this.AddNewPlayerChannel(playerAction);
            }

            foreach (var gameEvent in this.Game.GetPlayerActionGameEvents(playerAction))
            {
                foreach (var channel in this.activeChannels)
                {
                    channel.Publish(gameEvent.EventName.ToString(), gameEvent);
                }
            }
        }

        private void AddNewPlayerChannel(PlayerAction playerAction)
        {
            var channelId = playerAction.ActionData["ChannelId"].ToString();
            var channel = this.channels.Get(channelId);

            foreach (var gameEvent in this.Game.AllGameEvents)
            {
                channel.Publish(gameEvent.EventName.ToString(), gameEvent);
            }

            this.activeChannels.Add(channel);
        }
    }
}
