using TombolaGame.Models;

namespace TombolaGame.Repositories.Contracts
{
    public interface IPlayerRepository
    {
        Task<Player> AddAsync(Player player);
        Task<IEnumerable<Player>> GetAllAsync();
        Task<Player?> GetByIdAsync(int id);
    }
}
