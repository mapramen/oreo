namespace Oreo.Games
{
    using static Oreo.Games.Constants;

    public interface IGameRunnerFactory
    {
        public IGameRunner Create(string gameId, GameName gameName);
    }
}
