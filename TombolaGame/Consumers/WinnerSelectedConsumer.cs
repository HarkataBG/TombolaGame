using MassTransit;
using Microsoft.EntityFrameworkCore;
using TombolaGame.Data;
using TombolaGame.Events;
using TombolaGame.Models;

public class WinnerSelectedConsumer : IConsumer<WinnerSelectedEvent>
{
    private readonly TombolaContext _dbContext;

    public WinnerSelectedConsumer(TombolaContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<WinnerSelectedEvent> context)
    {
        var message = context.Message;

        var tombola = await _dbContext.Tombolas
            .Include(t => t.Winners)
            .FirstOrDefaultAsync(t => t.Id == message.TombolaId);

        if (tombola == null)
        {
            Console.WriteLine($"[ERROR] Tombola {message.TombolaId} not found.");
            return;
        }

        // предотвратяване на дублиране
        if (tombola.Winners.Any(w => w.Id == message.PlayerId))
            return;

        tombola.Winners.Add(new Player
        {
            Id = message.PlayerId,
            Name = message.PlayerName
        });

        await _dbContext.SaveChangesAsync();

        Console.WriteLine($"[DB] Winner {message.PlayerName} saved for Tombola {tombola.Id}");
    }
}