using TombolaGame.Models;

namespace TombolaGame.Services;

public interface ITombolaService
{
    Task<Tombola> CreateTombolaAsync(string name);
    Task<IEnumerable<Tombola>> GetTombolasAsync();
    Task<Tombola?> AddPlayerToTombolaAsync(int tombolaId, Player player);
}