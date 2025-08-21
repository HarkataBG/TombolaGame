using System.ComponentModel.DataAnnotations;

namespace TombolaGame.Models.Mappers
{
    public class PlayerRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(1, 100, ErrorMessage = "Weight must be at least 1.")]
        public int Weight { get; set; } = 1;
    }
}
