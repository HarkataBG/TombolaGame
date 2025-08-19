using TombolaGame.Models;

namespace TombolaGame.Repositories;

public interface ITombolaRepository
{
    Task<Tombola> AddAsync(Tombola tombola);
    Task<IEnumerable<Tombola>> GetAllAsync();
    Task<Tombola?> GetByIdAsync(int id);
    Task UpdateAsync(Tombola tombola);
}