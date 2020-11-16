namespace Oreo.Games
{
    using Microsoft.Bot.Schema;
    using System;
    using static Oreo.Games.Constants;

    public class GameDetails
    {
        public Guid Id { get; }
        public GameName Name { get; }

        public GameDetails(Guid id, GameName name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
