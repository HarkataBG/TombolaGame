namespace TombolaGame.Models.Mappers
{
    public class AwardResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? TombolaId { get; set; } 
    }
}
