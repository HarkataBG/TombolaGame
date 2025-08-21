namespace TombolaGame.Models
{
    public class Tombola
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Player> Players { get; set; } = new();
        public List<Award> Awards { get; set; } = new();
        public List<Player> Winners { get; set; } = new();

        public int MinimumPlayers { get; set; } = 2;
        public int MaximumPlayers { get; set; } = 3;
        public int MinimumAwards { get; set; } = 1;
        public int MaximumAwards { get; set; } = 2;

        public string State { get; set; } = "Waiting";
        public string StrategyType { get; set; } = "OnePrizePerPlayer";
    }
}
