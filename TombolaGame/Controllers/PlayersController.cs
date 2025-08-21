using Microsoft.AspNetCore.Mvc;
using TombolaGame.Models.Mappers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly IPlayerService _playerService;

    public PlayersController(IPlayerService service)
    {
        _playerService = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPlayers()
    {
        var players = await _playerService.GetAllPlayersAsync();
        return Ok(players);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlayerById(int id)
    {
        var player = await _playerService.GetPlayerByIdAsync(id);
        return Ok(player);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlayer([FromBody] PlayerRequest request)
    {
        var created = await _playerService.CreatePlayerAsync(request);
        return CreatedAtAction(nameof(GetPlayerById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlayer(int id, [FromBody] PlayerRequest request)
    {
        var updated = await _playerService.UpdatePlayerAsync(id, request);
        return Accepted(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlayer(int id)
    {
        await _playerService.DeletePlayerAsync(id);
        return NoContent();
    }
}