using TombolaGame.Enums;

namespace TombolaGame.Models.Mappers
{
    public class TombolaResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public TombolaState State { get; set; }
        public StrategyType StrategyType { get; set; }

        public int MinimumPlayers { get; set; }
        public int MaximumPlayers { get; set; }
        public int MinimumAwards { get; set; }
        public int MaximumAwards { get; set; }

        public List<string> PlayerNames { get; set; } = new();
        public List<string> AwardNames { get; set; } = new();
        public List<string> WinnerNames { get; set; } = new();
    }
}
