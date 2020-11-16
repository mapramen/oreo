namespace Oreo.Games
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static Oreo.Games.Constants;

    public class TicTacToeGame : IGame
    {
        public string GameId { get; }
        public GameName GameName => GameName.TicTacToe;
        public GameStatus GameStatus { get; private set; }
        public IReadOnlyCollection<GameEvent> AllGameEvents => this.gameEvents;

        private List<GameEvent> gameEvents;
        private List<Player> players;
        private int currentTurnPlayerIndex;
        private int markedTilesCount;
        private int visitedTiles;

        private static readonly int[] winningTilesMasks = { 7, 56, 448, 73, 146, 292, 273, 84 };

        public TicTacToeGame(string gameId)
        {
            this.GameId = gameId;
            this.GameStatus = GameStatus.Created;
            this.gameEvents = new List<GameEvent>();
            this.players = new List<Player>();

            this.CreateAndAppendGameEvent(
                GameEventName.GameCreated,
                DefaultPlayerId,
                new Dictionary<string, object>
                {
                    { "GameId", this.GameId },
                    { "GameName", GameNameEnumToString(this.GameName) }
                });
        }

        public IEnumerable<GameEvent> GetPlayerActionGameEvents(PlayerAction playerAction)
        {
            IEnumerable<GameEvent> gameEvents = Enumerable.Empty<GameEvent>();

            switch (playerAction.ActionName)
            {
                case PlayerActionName.AddMe:
                    gameEvents = this.GetAddMeActionGameEvents(playerAction);
                    break;
                case PlayerActionName.Move:
                    gameEvents = this.GetMoveActionGameEvents(playerAction);
                    break;
                default:
                    break;
            }

            return gameEvents;
        }

        private IEnumerable<GameEvent> GetAddMeActionGameEvents(PlayerAction playerAction)
        {
            if (this.GameStatus != GameStatus.Created
                || this.players.Any(player => player.PlayerId == playerAction.PlayerId))
            {
                yield break;
            }

            var newPlayer = new Player
            {
                PlayerId = playerAction.PlayerId,
                Name = playerAction.ActionData["PlayerName"].ToString(),
                Team = this.GetNewPlayerTeam(),
                SelectedTilesMask = 0
            };

            this.players.Add(newPlayer);

            yield return this.CreateAndAppendGameEvent(
                GameEventName.NewPlayer,
                DefaultPlayerId,
                new Dictionary<string, object> { { "Player", newPlayer } });

            if (this.players.Count != 2)
            {
                yield break;
            }

            yield return this.CreateAndAppendGameEvent(
                GameEventName.StartGame,
                DefaultPlayerId,
                null);

            this.GameStatus = GameStatus.Started;
            this.currentTurnPlayerIndex = new Random().Next(2);
            this.markedTilesCount = 0;
            this.visitedTiles = 0;

            yield return this.GetNextTurnGameEvent();
        }

        private IEnumerable<GameEvent> GetMoveActionGameEvents(PlayerAction playerAction)
        {
            var currentTurnPlayer = this.players[this.currentTurnPlayerIndex];
            int tileNumber = (int)playerAction.ActionData["TileNumber"];

            if (this.GameStatus == GameStatus.Created
                || this.markedTilesCount == 9
                || currentTurnPlayer.PlayerId != playerAction.PlayerId
                || (this.visitedTiles & (1 << tileNumber)) > 0)
            {
                yield break;
            }

            ++this.markedTilesCount;
            currentTurnPlayer.SelectedTilesMask |= (1 << tileNumber);

            if (this.DidCurrentTurnPlayerWin())
            {
                this.GameStatus = GameStatus.Completed;
                yield return this.CreateAndAppendGameEvent(
                    GameEventName.PlayerWon,
                    currentTurnPlayer.PlayerId,
                    null);
            }

            if(this.markedTilesCount == 9)
            {
                this.GameStatus = GameStatus.Completed;
                yield return this.CreateAndAppendGameEvent(
                    GameEventName.GameDraw,
                    DefaultPlayerId,
                    null);
            }

            yield return this.CreateAndAppendGameEvent(
                GameEventName.PlayerMove,
                currentTurnPlayer.PlayerId,
                new Dictionary<string, object>
                {
                    {"TileNumber", tileNumber },
                    {"TileMark", currentTurnPlayer.Team }
                });

            yield return this.GetNextTurnGameEvent();
        }

        private bool DidCurrentTurnPlayerWin()
        {
            int selectedTilesMask = 
                this.players[this.currentTurnPlayerIndex].SelectedTilesMask;

            return TicTacToeGame.winningTilesMasks
                .Any(winningTilesMask => (winningTilesMask & selectedTilesMask) == winningTilesMask);
        }

        private string GetNewPlayerTeam()
        {
            if(this.players.Count == 0)
            {
                var rand = new Random();
                return rand.NextDouble() < 0.5 ? "X" : "O";
            }

            return players.First().Team == "X" ? "O" : "X";
        }

        private GameEvent GetNextTurnGameEvent()
        {
            this.currentTurnPlayerIndex ^= 1;

            return this.CreateAndAppendGameEvent(
                GameEventName.NextPlayerTurn,
                this.players[this.currentTurnPlayerIndex ^ 1].PlayerId,
                null);
        }

        private GameEvent CreateAndAppendGameEvent(
            GameEventName eventName,
            string playerId,
            IReadOnlyDictionary<string, object> eventData)
        {
            var gameEvent = new GameEvent
            {
                GameId = this.GameId,
                EventName = eventName,
                PlayerId = playerId,
                EventData = eventData
            };

            this.gameEvents.Add(gameEvent);

            return gameEvent;
        }
    }
}
