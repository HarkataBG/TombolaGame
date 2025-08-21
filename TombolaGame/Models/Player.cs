
namespace TombolaGame.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Weight { get; set; } = 1;
        public ICollection<Tombola> Tombolas { get; set; } = new List<Tombola>();
    }
}
