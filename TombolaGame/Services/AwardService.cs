using TombolaGame.Models;
using TombolaGame.Repositories;

namespace TombolaGame.Services;

public class AwardService : IAwardService
{
    private readonly IAwardRepository _awardRepo;
    private readonly ITombolaRepository _tombolaRepo;

    public AwardService(IAwardRepository awardRepo, ITombolaRepository tombolaRepo)
    {
        _awardRepo = awardRepo;
        _tombolaRepo = tombolaRepo;
    }

    public async Task<Award> CreateAwardAsync(string name)
    {
        var award = new Award { Name = name };
        return await _awardRepo.AddAsync(award);
    }

    public async Task<IEnumerable<Award>> GetAwardsAsync()
    {
        return await _awardRepo.GetAllAsync();
    }

    public async Task<Award?> AssignToTombolaAsync(int awardId, int tombolaId)
    {
        var award = await _awardRepo.GetByIdAsync(awardId);
        var tombola = await _tombolaRepo.GetByIdAsync(tombolaId);

        if (award == null || tombola == null) return null;

        award.TombolaId = tombolaId;
        await _awardRepo.UpdateAsync(award);
        return award;
    }
}