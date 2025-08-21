namespace TombolaGame.Models
{
    public class TombolaWinner
    {
        public int Id { get; set; }
        public int TombolaId { get; set; }
        public Tombola Tombola { get; set; } = null!;

        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;
    }
}
