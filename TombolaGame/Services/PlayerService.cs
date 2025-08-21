using TombolaGame.Exceptions;
using TombolaGame.Helpers;
using TombolaGame.Models;
using TombolaGame.Models.Mappers;
using TombolaGame.Repositories.Contracts;

namespace TombolaGame.Services;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<IEnumerable<PlayerResponse>> GetAllPlayersAsync()
    {
        var players = await _playerRepository.GetAllAsync();
        return players.Select(ModelMapper.ToResponse);
    }

    public async Task<PlayerResponse> GetPlayerByIdAsync(int id)
    {
        var player = await _playerRepository.GetByIdAsync(id)
            ?? throw new EntityNotFoundException("Player", id);

        return ModelMapper.ToResponse(player);
    }

    public async Task<PlayerResponse> CreatePlayerAsync(PlayerRequest request)
    {
        var player = new Player
        {
            Name = request.Name,
            Weight = request.Weight
        };

        await _playerRepository.AddAsync(player);
        return ModelMapper.ToResponse(player);
    }

    public async Task<PlayerResponse> UpdatePlayerAsync(int id, PlayerRequest request)
    {
        var player = await _playerRepository.GetByIdAsync(id)
            ?? throw new EntityNotFoundException("Player", id);

        player.Name = request.Name;
        player.Weight = request.Weight;

        await _playerRepository.UpdateAsync(player);
        return ModelMapper.ToResponse(player);
    }

    public async Task DeletePlayerAsync(int id)
    {
        var player = await _playerRepository.GetByIdAsync(id)
            ?? throw new EntityNotFoundException("Player", id);

        await _playerRepository.DeleteAsync(player);
    }

}