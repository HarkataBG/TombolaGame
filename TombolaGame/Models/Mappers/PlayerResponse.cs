namespace TombolaGame.Models.Mappers
{
    public class PlayerResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Weight { get; set; }
        public List<int> TombolaIds { get; set; } = new();
    }
}
