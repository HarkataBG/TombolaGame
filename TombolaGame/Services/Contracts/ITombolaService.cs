using TombolaGame.Models;

namespace TombolaGame.Services;

public interface ITombolaService
{
    Task<Tombola> CreateTombolaAsync(string name, string strategyType);
    Task<IEnumerable<Tombola>> GetTombolasAsync();
    Task<Tombola?> AddPlayerToTombolaAsync(int tombolaId, Player player);
    Task<Tombola> GetTombolaById(int tombolaId);
}