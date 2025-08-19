using TombolaGame.Models;
using TombolaGame.Repositories.Contracts;

namespace TombolaGame.Services;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerService(IPlayerRepository repository) => _playerRepository = repository;

    public async Task<Player> CreatePlayerAsync(string name, int weight = 1)
    {
        var player = new Player { Name = name, Weight = weight };
        return await _playerRepository.AddAsync(player);
    }

    public async Task<IEnumerable<Player>> GetPlayersAsync()
    {
        return await _playerRepository.GetAllAsync();
    }

    public async Task<Player?> GetPlayerAsync(int id)
    {
        return await _playerRepository.GetByIdAsync(id);
    }
}