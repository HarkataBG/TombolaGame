using Microsoft.AspNetCore.Mvc;
using TombolaGame.Exceptions;
using TombolaGame.Models.Mappers;
using TombolaGame.Services;

namespace TombolaGame.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TombolasController : ControllerBase
{
    private readonly ITombolaService _tombolaService;

    public TombolasController(ITombolaService tombolaService)
    {
        _tombolaService = tombolaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTombolas()
    {
        var tombolas = await _tombolaService.GetAllTombolasAsync();
        return Ok(tombolas);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTombolaById(int id)
    {
        var tombola = await _tombolaService.GetTombolaByIdAsync(id);
        return Ok(tombola);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTombola([FromBody] TombolaRequest request)
    {
        var created = await _tombolaService.CreateTombolaAsync(request);
        return CreatedAtAction(nameof(GetTombolas), new { id = created.Id }, created);
    }

    [HttpPut("{tombolaId}")]
    public async Task<IActionResult> UpdateTombola(int tombolaId, [FromBody] TombolaRequest request)
    {
        var updated = await _tombolaService.UpdateTombolaAsync(tombolaId, request);
        return Accepted(updated);
    }

    [HttpDelete("{tombolaId}")]
    public async Task<IActionResult> DeleteTombola(int tombolaId)
    {
        await _tombolaService.DeleteTombolaAsync(tombolaId);
        return Accepted(new { message = "Tombola deleted successfully." });
    }

    [HttpPost("{tombolaId}/join")]
    public async Task<IActionResult> JoinTombola(int tombolaId, [FromBody] JoinTombolaRequest request)
    {
        await _tombolaService.JoinTombolaAsync(tombolaId, request.PlayerName);
        return Ok(new { message = "Player successfully joined the tombola." });
    }

    [HttpPost("{tombolaId}/awards/assign")]
    public async Task<IActionResult> AssignAward(int tombolaId, [FromBody] AssignAwardRequest request)
    {
        if (request == null || request.AwardId <= 0)
            return BadRequest(new { message = "AwardId is required." });

        await _tombolaService.AssignAwardAsync(tombolaId, request.AwardId);

        return Ok(new { message = "Award assigned successfully." });
    }

    [HttpPost("{tombolaId}/draw")]
    public async Task<IActionResult> DrawWinners(int tombolaId)
    {
        var tombola = await _tombolaService.GetTombolaByIdAsync(tombolaId);
        if (tombola == null)
            throw new EntityNotFoundException("Tombola", tombolaId);

        if (!tombola.PlayerNames.Any() || !tombola.AwardNames.Any())
            throw new InvalidOperationException("Cannot draw: no players or no awards.");

        await _tombolaService.StartTombolaAsync(tombolaId);

        var updatedTombola = await _tombolaService.GetTombolaByIdAsync(tombolaId);

        return Ok(updatedTombola!.Winners);
    }

}
