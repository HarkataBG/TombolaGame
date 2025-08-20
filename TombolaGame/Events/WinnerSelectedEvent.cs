namespace TombolaGame.Events
{
    public class WinnerSelectedEvent
    {
        public int TombolaId { get; }
        public int PlayerId { get; }
        public string PlayerName { get; }

        public WinnerSelectedEvent(int tombolaId, int playerId, string playerName)
        {
            TombolaId = tombolaId;
            PlayerId = playerId;
            PlayerName = playerName;
        }
    }
}