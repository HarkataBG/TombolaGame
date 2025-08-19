using Microsoft.EntityFrameworkCore;
using TombolaGame.Data;
using TombolaGame.Models;

namespace TombolaGame.Repositories;

public class TombolaRepository : ITombolaRepository
{
    private readonly TombolaContext _context;
    public TombolaRepository(TombolaContext context) => _context = context;

    public async Task<Tombola> AddAsync(Tombola tombola)
    {
        _context.Tombolas.Add(tombola);
        await _context.SaveChangesAsync();
        return tombola;
    }

    public async Task<IEnumerable<Tombola>> GetAllAsync()
    {
        return await _context.Tombolas
            .Include(t => t.Players)
            .Include(t => t.Awards)
            .ToListAsync();
    }

    public async Task<Tombola?> GetByIdAsync(int id)
    {
        return await _context.Tombolas
            .Include(t => t.Players)
            .Include(t => t.Awards)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task UpdateAsync(Tombola tombola)
    {
        _context.Tombolas.Update(tombola);
        await _context.SaveChangesAsync();
    }
}