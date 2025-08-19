using TombolaGame.Models;

namespace TombolaGame.Repositories;

public interface IAwardRepository
{
    Task<Award> AddAsync(Award award);
    Task<IEnumerable<Award>> GetAllAsync();
    Task<Award?> GetByIdAsync(int id);
    Task UpdateAsync(Award award);
}