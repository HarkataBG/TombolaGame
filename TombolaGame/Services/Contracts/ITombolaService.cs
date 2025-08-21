using TombolaGame.Models;
using TombolaGame.Models.Mappers;

namespace TombolaGame.Services
{
    public interface ITombolaService
    {
        Task<TombolaResponse> GetTombolaByIdAsync(int tombolaId);

        Task<IEnumerable<TombolaResponse>> GetAllTombolasAsync();

        Task<TombolaResponse> CreateTombolaAsync(TombolaRequest request);

        Task<TombolaResponse> UpdateTombolaAsync(int tombolaId, TombolaRequest request);

        Task DeleteTombolaAsync(int tombolaId);

        Task JoinTombolaAsync(int tombolaId, string playerName);

        Task AssignAwardAsync(int tombolaId, int awardId);

        Task StartTombolaAsync(int tombolaId);
        
    }
}