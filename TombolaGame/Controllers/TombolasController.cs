using Microsoft.AspNetCore.Mvc;
using TombolaGame.Services;
using TombolaGame.Models;
using TombolaGame.WinnerSelection;

namespace TombolaGame.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TombolasController : ControllerBase
{
    private readonly ITombolaService _tombolaService;
    private readonly IWinnerSelectionService _winnerService;

    public TombolasController(ITombolaService tombolaService, IWinnerSelectionService winnerService)
    {
        _tombolaService = tombolaService;
        _winnerService = winnerService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTombola([FromBody] Tombola tombola)
    {
        var created = await _tombolaService.CreateTombolaAsync(tombola.Name, tombola.StrategyType);
        return CreatedAtAction(nameof(GetTombolas), new { id = created.Id }, created);
    }

    [HttpGet]
    public async Task<IActionResult> GetTombolas()
    {
        var tombolas = await _tombolaService.GetTombolasAsync();
        return Ok(tombolas);
    }

    [HttpPost("{tombolaId}/players")]
    public async Task<IActionResult> AddPlayer(int tombolaId, [FromBody] Player player)
    {
        var updated = await _tombolaService.AddPlayerToTombolaAsync(tombolaId, player);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpPost("{tombolaId}/draw")]
    public async Task<IActionResult> DrawWinners(int tombolaId)
    {
        var tombola = await _tombolaService.GetTombolaById(tombolaId);
        if (tombola == null) return NotFound();

        if (!tombola.Players.Any() || !tombola.Awards.Any())
            return BadRequest("Cannot draw: no players or no awards.");

        var winners = await _winnerService.DrawWinnersAsync(tombola);

        var response = winners.Select(w => new
        {
            PlayerId = w.Id,
            PlayerName = w.Name
        });

        return Ok(response);
    }

    [HttpPost("{tombolaId}/draw/single")]
    public async Task<IActionResult> DrawSingleWinner(int tombolaId)
    {
        var tombola = await _tombolaService.GetTombolaById(tombolaId);
        if (tombola == null) return NotFound();

        if (!tombola.Players.Any() || !tombola.Awards.Any())
            return BadRequest("Cannot draw: no players or no awards.");

        var winner = await _winnerService.DrawWinnerAsync(tombola);

        if (winner == null)
            return BadRequest("No eligible winner could be selected.");

        return Ok(new
        {
            PlayerId = winner.Id,
            PlayerName = winner.Name
        });
    }

}
