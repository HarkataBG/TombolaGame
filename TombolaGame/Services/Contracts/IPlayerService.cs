using TombolaGame.Models.Mappers;

public interface IPlayerService
{
    Task<IEnumerable<PlayerResponse>> GetAllPlayersAsync();
    Task<PlayerResponse> GetPlayerByIdAsync(int id);
    Task<PlayerResponse> CreatePlayerAsync(PlayerRequest request);
    Task<PlayerResponse> UpdatePlayerAsync(int id, PlayerRequest request);
    Task DeletePlayerAsync(int id);
}