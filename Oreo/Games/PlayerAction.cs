namespace Oreo.Games
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Collections.Generic;
    using static Oreo.Games.Constants;

    public class PlayerAction
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public PlayerActionName ActionName { get; set; }
        public string PlayerId { get; set; }
        public IReadOnlyDictionary<string, object> ActionData { get; set; }
    }
}
