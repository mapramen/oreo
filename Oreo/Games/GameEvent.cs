namespace Oreo.Games
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Collections.Generic;
    using static Oreo.Games.Constants;

    public class GameEvent
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public GameEventName EventName { get; set; }
        public string GameId { get; set; }
        public string PlayerId { get; set; }
        public IReadOnlyDictionary<string, object> EventData { get; set; }
    }
}
