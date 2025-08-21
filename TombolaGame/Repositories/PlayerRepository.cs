using MassTransit;
using Microsoft.EntityFrameworkCore;
using TombolaGame.Data;
using TombolaGame.Models;
using TombolaGame.Repositories.Contracts;

public class PlayerRepository : IPlayerRepository
{
    private readonly TombolaContext _context;

    public PlayerRepository(TombolaContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Player>> GetAllAsync()
    {
        return await _context.Players
            .Include(p => p.Tombolas)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Player?> GetByNameAsync(string name)
    {
        return await _context.Players
            .FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task<Player?> GetByIdAsync(int id)
    {
        return await _context.Players
            .Include(p => p.Tombolas)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Player player)
    {
        _context.Players.Add(player);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Player player)
    {
        _context.Players.Update(player);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Player player)
    {
        _context.Players.Remove(player);
        await _context.SaveChangesAsync();
    }
}