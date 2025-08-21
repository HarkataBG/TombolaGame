using System.ComponentModel.DataAnnotations;

namespace TombolaGame.Models.Mappers
{
    public class JoinTombolaRequest
    {
        [Required(ErrorMessage = "Player name is required.")]
        public string PlayerName { get; set; } = null!;
    }
}
