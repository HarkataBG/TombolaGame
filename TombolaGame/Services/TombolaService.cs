using TombolaGame.Models;
using TombolaGame.Repositories.Contracts;
using TombolaGame.Repositories;

namespace TombolaGame.Services;

public class TombolaService : ITombolaService
{
    private readonly ITombolaRepository _tombolaRepository;
    private readonly IPlayerRepository _playerRepository;

    public TombolaService(ITombolaRepository tombolaRepository, IPlayerRepository playerRepository)
    {
        _tombolaRepository = tombolaRepository;
        _playerRepository = playerRepository;
    }

    public async Task<Tombola> CreateTombolaAsync(string name)
    {
        var tombola = new Tombola { Name = name };
        return await _tombolaRepository.AddAsync(tombola);
    }

    public async Task<IEnumerable<Tombola>> GetTombolasAsync()
    {
        return await _tombolaRepository.GetAllAsync();
    }

    public async Task<Tombola?> AddPlayerToTombolaAsync(int tombolaId, Player player)
    {
        var tombola = await _tombolaRepository.GetByIdAsync(tombolaId);
        if (tombola == null) return null;

        var existingPlayer = await _playerRepository.GetByIdAsync(player.Id);
        if (existingPlayer == null)
        {
            existingPlayer = await _playerRepository.AddAsync(player);
        }

        tombola.Players.Add(existingPlayer);
        await _tombolaRepository.UpdateAsync(tombola);

        return tombola;
    }
}
