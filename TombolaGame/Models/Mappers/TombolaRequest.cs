using System.ComponentModel.DataAnnotations;

namespace TombolaGame.Models.Mappers
{
    public class TombolaRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int MinAwards { get; set; }
        public int MaxAwards { get; set; }
        public string? StrategyType { get; set; } = "OnePrizePerPlayer";
    }
}
