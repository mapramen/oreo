namespace Oreo.Games
{
    using System;

    public static class Constants
    {
        public const string DefaultPlayerId = "00000000-00000000-00000000-00000000";
        private const string TicTacToe = "Tic-Tac-Toe";
        private const string Codenames = "Codenames";
        private const string CodenamesDuet = "Codenames (Duet)";

        public enum GameName
        {
            TicTacToe,
            Codenames,
            CodenamesDuet
        }

        public enum PlayerActionName
        {
            AddMe,
            Move
        }

        public enum GameEventName
        {
            GameCreated,
            NewPlayer,
            StartGame,
            PlayerMove,
            NextPlayerTurn,
            PlayerWon,
            GameDraw
        }

        public enum GameStatus
        {
            Created,
            Started,
            Completed
        }

        public static string GameNameEnumToString(GameName gameName)
        {
            switch (gameName)
            {
                case GameName.TicTacToe:
                    return TicTacToe;
                case GameName.Codenames:
                    return Codenames;
                case GameName.CodenamesDuet:
                    return CodenamesDuet;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameName));
            }
        }

        public static GameName GameNameStringToEnum(string gameNameString)
        {
            switch (gameNameString)
            {
                case TicTacToe:
                    return GameName.TicTacToe;
                case Codenames:
                    return GameName.Codenames;
                case CodenamesDuet:
                    return GameName.CodenamesDuet;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameNameString));
            }
        }
    }
}
