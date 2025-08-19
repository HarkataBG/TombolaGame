using Microsoft.AspNetCore.Mvc;
using TombolaGame.Models;
using TombolaGame.Services;

namespace TombolaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AwardsController : ControllerBase
{
    private readonly IAwardService _service;

    public AwardsController(IAwardService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> AddAward([FromBody] Award award)
    {
        var created = await _service.CreateAwardAsync(award.Name);
        return CreatedAtAction(nameof(GetAwards), new { id = created.Id }, created);
    }

    [HttpGet]
    public async Task<IActionResult> GetAwards()
    {
        var awards = await _service.GetAwardsAsync();
        return Ok(awards);
    }

    [HttpPost("{awardId}/assign/{tombolaId}")]
    public async Task<IActionResult> AssignToTombola(int awardId, int tombolaId)
    {
        var updated = await _service.AssignToTombolaAsync(awardId, tombolaId);
        if (updated == null) return NotFound();
        return Ok(updated);
    }
}