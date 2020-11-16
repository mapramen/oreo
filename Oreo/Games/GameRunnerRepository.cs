namespace Oreo.Games
{
    using System.Collections.Concurrent;

    public class GameRunnerRepository : IGameRunnerRepository
    {
        private IGameRunnerFactory gameRunnerFactory;
        private ConcurrentDictionary<string, IGameRunner> gameIdToGameRunners;

        public GameRunnerRepository(IGameRunnerFactory gameRunnerFactory)
        {
            this.gameRunnerFactory = gameRunnerFactory;
            this.gameIdToGameRunners = new ConcurrentDictionary<string, IGameRunner>();
        }

        public void Add(string gameId, Constants.GameName gameName)
        {
            this.gameIdToGameRunners.TryAdd(
                gameId,
                this.gameRunnerFactory.Create(gameId, gameName));
        }

        public IGameRunner Get(string gameId)
        {
            return this.gameIdToGameRunners.GetOrAdd(
                gameId,
                this.gameRunnerFactory.Create(gameId, Constants.GameName.TicTacToe));
        }
    }
}
