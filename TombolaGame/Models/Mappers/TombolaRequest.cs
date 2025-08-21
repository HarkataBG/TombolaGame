using TombolaGame.Enums;

namespace TombolaGame.Models.Mappers
{
    public class TombolaRequest
    {
        public string Name { get; set; } = null!;
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int MinAwards { get; set; }
        public int MaxAwards { get; set; }
        public StrategyType? StrategyType { get; set; } 
    }
}
