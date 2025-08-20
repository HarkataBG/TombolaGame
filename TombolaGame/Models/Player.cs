
namespace TombolaGame.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Weight { get; set; } = 1;

        internal object Select(Func<object, object> value)
        {
            throw new NotImplementedException();
        }
    }
}
