namespace Oreo.Games
{
    using IO.Ably;
    using IO.Ably.Realtime;
    using System;
    using static Oreo.Games.Constants;
    
    public class GameRunnerFactory : IGameRunnerFactory
    {
        private readonly IChannels<IRealtimeChannel> channels;

        public GameRunnerFactory(IChannels<IRealtimeChannel> channels)
        {
            this.channels = channels;
        }

        public IGameRunner Create(string gameId, GameName gameName)
        {
            return new GameRunner(
                this.CreateGame(gameId, gameName),
                channels);
        }

        private IGame CreateGame(string gameId, GameName gameName)
        {
            switch (gameName)
            {
                case GameName.TicTacToe:
                    return new TicTacToeGame(gameId);
                case GameName.Codenames:
                case GameName.CodenamesDuet:
                default:
                    throw new NotImplementedException($"{gameName} is not implemented");
            }
        }
    }
}
