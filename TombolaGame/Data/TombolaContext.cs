using Microsoft.EntityFrameworkCore;
using TombolaGame.Models;

namespace TombolaGame.Data;

public class TombolaContext : DbContext
{
    public TombolaContext(DbContextOptions<TombolaContext> options) : base(options) { }
    public DbSet<Player> Players => Set<Player>();
    public DbSet<Award> Awards => Set<Award>();
    public DbSet<Tombola> Tombolas => Set<Tombola>();
}