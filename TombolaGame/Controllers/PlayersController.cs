using Microsoft.AspNetCore.Mvc;
using TombolaGame.Models;
using TombolaGame.Services;

namespace TombolaGame.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly IPlayerService _service;

    public PlayersController(IPlayerService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> AddPlayer([FromBody] Player player)
    {
        var created = await _service.CreatePlayerAsync(player.Name, player.Weight);
        return CreatedAtAction(nameof(GetPlayer), new { id = created.Id }, created);
    }

    [HttpGet]
    public async Task<IActionResult> GetPlayers()
    {
        var players = await _service.GetPlayersAsync();
        return Ok(players);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlayer(int id)
    {
        var player = await _service.GetPlayerAsync(id);
        if (player == null) return NotFound();
        return Ok(player);
    }
}