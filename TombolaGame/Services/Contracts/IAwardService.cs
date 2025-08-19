using TombolaGame.Models;

namespace TombolaGame.Services;

public interface IAwardService
{
    Task<Award> CreateAwardAsync(string name);
    Task<IEnumerable<Award>> GetAwardsAsync();
    Task<Award?> AssignToTombolaAsync(int awardId, int tombolaId);
}