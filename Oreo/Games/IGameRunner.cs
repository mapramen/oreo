namespace Oreo.Games
{
    public interface IGameRunner
    {
        public IGame Game { get; }
        public string GetPlayerChannelId();
    }
}
