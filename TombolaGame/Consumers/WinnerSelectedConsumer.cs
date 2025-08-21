using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using TombolaGame.Data;
using TombolaGame.Events;
using TombolaGame.Models;

public class WinnerSelectedConsumer : IConsumer<WinnerSelectedEvent>
{
    private readonly TombolaContext _dbContext;
    private readonly ILogger<WinnerSelectedConsumer> _logger;

    public WinnerSelectedConsumer(TombolaContext dbContext, ILogger<WinnerSelectedConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<WinnerSelectedEvent> context)
    {
        var message = context.Message;

        var tombola = await _dbContext.Tombolas
            .Include(t => t.Winners)
            .FirstOrDefaultAsync(t => t.Id == message.TombolaId);

        if (tombola == null)
        {
            _logger.LogError("Tombola {TombolaId} not found.", message.TombolaId);
            return;
        }

        if (tombola.Winners.Any(w => w.Id == message.PlayerId))
        {
            _logger.LogWarning("Winner {PlayerId} already exists for Tombola {TombolaId}.", message.PlayerId, tombola.Id);
            return;
        }

        tombola.Winners.Add(new Player
        {
            Id = message.PlayerId,
            Name = message.PlayerName
        });

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Winner {PlayerName} saved for Tombola {TombolaId}.", message.PlayerName, tombola.Id);
    }
}