using Microsoft.EntityFrameworkCore;
using TombolaGame.Data;
using TombolaGame.Models;
using TombolaGame.Repositories.Contracts;

namespace TombolaGame.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly TombolaContext _context;
    public PlayerRepository(TombolaContext context) => _context = context;

    public async Task<Player> AddAsync(Player player)
    {
        _context.Players.Add(player);
        await _context.SaveChangesAsync();
        return player;
    }

    public async Task<IEnumerable<Player>> GetAllAsync()
    {
        return await _context.Players.ToListAsync();
    }

    public async Task<Player?> GetByIdAsync(int id)
    {
        return await _context.Players.FindAsync(id);
    }
}