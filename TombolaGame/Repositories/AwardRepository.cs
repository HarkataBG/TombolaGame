using Microsoft.EntityFrameworkCore;
using TombolaGame.Data;
using TombolaGame.Models;

namespace TombolaGame.Repositories;

public class AwardRepository : IAwardRepository
{
    private readonly TombolaContext _context;
    public AwardRepository(TombolaContext context) => _context = context;

    public async Task<Award> AddAsync(Award award)
    {
        _context.Awards.Add(award);
        await _context.SaveChangesAsync();
        return award;
    }

    public async Task<IEnumerable<Award>> GetAllAsync()
    {
        return await _context.Awards.ToListAsync();
    }

    public async Task<Award?> GetByIdAsync(int id)
    {
        return await _context.Awards.FindAsync(id);
    }

    public async Task UpdateAsync(Award award)
    {
        _context.Awards.Update(award);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Award award)
    {
        _context.Awards.Remove(award);
        await _context.SaveChangesAsync();
    }
}