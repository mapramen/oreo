namespace Oreo.Games
{
    using static Oreo.Games.Constants;

    public interface IGameRunnerRepository
    {
        public void Add(string gameId, GameName gameName);
        public IGameRunner Get(string gameId);
    }
}
