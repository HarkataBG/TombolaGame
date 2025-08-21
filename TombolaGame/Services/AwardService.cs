using TombolaGame.Exceptions;
using TombolaGame.Helpers;
using TombolaGame.Models;
using TombolaGame.Models.Mappers;
using TombolaGame.Repositories;
using TombolaGame.Services;

public class AwardService : IAwardService
{
    private readonly IAwardRepository _awardRepository;

    public AwardService(IAwardRepository awardRepository)
    {
        _awardRepository = awardRepository;
    }

    public async Task<IEnumerable<AwardResponse>> GetAllAwardsAsync()
    {
        var awards = await _awardRepository.GetAllAsync();
        return awards.Select(ModelMapper.ToResponse);
    }

    public async Task<AwardResponse> GetAwardByIdAsync(int id)
    {
        var award = await _awardRepository.GetByIdAsync(id);
        if (award == null)
            throw new EntityNotFoundException("Award", id);

        return ModelMapper.ToResponse(award);
    }

    public async Task<AwardResponse> CreateAwardAsync(AwardRequest request)
    {
        var award = new Award
        {
            Name = request.Name
        };

        await _awardRepository.AddAsync(award);

        return ModelMapper.ToResponse(award);
    }

    public async Task<AwardResponse> UpdateAwardAsync(int id, AwardRequest request)
    {
        var award = await _awardRepository.GetByIdAsync(id);
        if (award == null)
            throw new EntityNotFoundException("Award", id);

        award.Name = request.Name;

        await _awardRepository.UpdateAsync(award);

        return ModelMapper.ToResponse(award);
    }

    public async Task DeleteAwardAsync(int id)
    {
        var award = await _awardRepository.GetByIdAsync(id)
            ?? throw new EntityNotFoundException("Award", id);

        if (award.TombolaId != null)
            throw new InvalidOperationException("Cannot delete award assigned to a tombola.");

        await _awardRepository.DeleteAsync(award);
    }
}