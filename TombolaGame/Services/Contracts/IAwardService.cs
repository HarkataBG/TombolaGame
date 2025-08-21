using TombolaGame.Models.Mappers;

namespace TombolaGame.Services;

public interface IAwardService
{
    Task<IEnumerable<AwardResponse>> GetAllAwardsAsync();
    Task<AwardResponse> GetAwardByIdAsync(int id);
    Task<AwardResponse> CreateAwardAsync(AwardRequest request);
    Task<AwardResponse> UpdateAwardAsync(int id, AwardRequest request);
    Task DeleteAwardAsync(int id);
}