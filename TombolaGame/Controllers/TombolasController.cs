using Microsoft.AspNetCore.Mvc;
using TombolaGame.Services;
using TombolaGame.Models;

namespace TombolaGame.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TombolasController : ControllerBase
{
    private readonly ITombolaService _service;

    public TombolasController(ITombolaService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> CreateTombola([FromBody] Tombola tombola)
    {
        var created = await _service.CreateTombolaAsync(tombola.Name);
        return CreatedAtAction(nameof(GetTombolas), new { id = created.Id }, created);
    }

    [HttpGet]
    public async Task<IActionResult> GetTombolas()
    {
        var tombolas = await _service.GetTombolasAsync();
        return Ok(tombolas);
    }

    [HttpPost("{tombolaId}/players")]
    public async Task<IActionResult> AddPlayer(int tombolaId, [FromBody] Player player)
    {
        var updated = await _service.AddPlayerToTombolaAsync(tombolaId, player);
        if (updated == null) return NotFound();
        return Ok(updated);
    }
}
