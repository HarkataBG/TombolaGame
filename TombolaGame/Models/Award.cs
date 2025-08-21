namespace TombolaGame.Models
{
    public class Award
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int? TombolaId { get; set; }
        public Tombola? Tombola { get; set; }
    }
}
