namespace TombolaGame.Events
{
    public class WinnerSelectedEvent
    {
        public int TombolaId { get; }
        public int PlayerId { get; }
        public string PlayerName { get; }
        public int AwardId { get; }


        public WinnerSelectedEvent(int tombolaId, int playerId, string playerName, int awardId)
        {
            TombolaId = tombolaId;
            PlayerId = playerId;
            PlayerName = playerName;
            AwardId = awardId;
        }
    }
}