using Microsoft.EntityFrameworkCore;
using TombolaGame.Data;
using TombolaGame.Models;

namespace TombolaGame.Repositories;

public class TombolaRepository : ITombolaRepository
{
    private readonly TombolaContext _context;
    public TombolaRepository(TombolaContext context) => _context = context;   

    public async Task<IEnumerable<Tombola>> GetAllAsync()
    {
        return await _context.Tombolas
            .Include(t => t.Players)
            .Include(t => t.Awards)
            .Include(t => t.Winners)
            .ToListAsync();
    }

    public async Task<Tombola?> GetByIdAsync(int id)
    {
        return await _context.Tombolas
            .Include(t => t.Players)
            .Include(t => t.Awards)
            .Include(t => t.Winners)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Tombola> AddAsync(Tombola tombola)
    {
        _context.Tombolas.Add(tombola);
        await _context.SaveChangesAsync();
        return tombola;
    }

    public async Task UpdateAsync(Tombola tombola)
    {
        _context.Tombolas.Update(tombola);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Tombola tombola)
    {
        _context.Tombolas.Remove(tombola);
        await _context.SaveChangesAsync();
    }
}