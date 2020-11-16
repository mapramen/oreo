namespace Oreo.Games
{
    using System.Collections.Generic;
    using static Oreo.Games.Constants;

    public interface IGame
    {
        public string GameId { get; }
        public GameName GameName { get; }
        public GameStatus GameStatus { get; }
        public IReadOnlyCollection<GameEvent> AllGameEvents { get; }
        public IEnumerable<GameEvent> GetPlayerActionGameEvents(PlayerAction playerAction);
    }
}
