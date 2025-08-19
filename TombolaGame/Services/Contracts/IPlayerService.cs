using TombolaGame.Models;

namespace TombolaGame.Services;

public interface IPlayerService
{
    Task<Player> CreatePlayerAsync(string name, int weight = 1);
    Task<IEnumerable<Player>> GetPlayersAsync();
    Task<Player?> GetPlayerAsync(int id);
}