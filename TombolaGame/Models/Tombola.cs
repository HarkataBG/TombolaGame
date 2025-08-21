using MassTransit;
using TombolaGame.Enums;

namespace TombolaGame.Models
{
    public class Tombola
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<Player> Players { get; set; } = new List<Player>();
        public ICollection<Award> Awards { get; set; } = new List<Award>();
        public ICollection<TombolaWinner> Winners { get; set; } = new List<TombolaWinner>();

        public int MinimumPlayers { get; set; } = 2;
        public int MaximumPlayers { get; set; } = 3;
        public int MinimumAwards { get; set; } = 1;
        public int MaximumAwards { get; set; } = 2;

        public TombolaState State { get; set; } = TombolaState.Waiting;
        public StrategyType StrategyType { get; set; } = StrategyType.OnePrizePerPlayer;
    }
}
