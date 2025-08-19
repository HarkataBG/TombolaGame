namespace TombolaGame.Models
{
    public class Tombola
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Player> Players { get; set; } = new();
        public List<Award> Awards { get; set; } = new();
        public string State { get; set; } = "Waiting";
    }
}
