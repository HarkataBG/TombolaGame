using Microsoft.AspNetCore.Mvc;
using TombolaGame.Services;

[ApiController]
[Route("api/[controller]")]
public class AwardsController : ControllerBase
{
    private readonly IAwardService _awardService;

    public AwardsController(IAwardService service) => _awardService = service;

    [HttpGet]
    public async Task<IActionResult> GetAllAwards()
    {
        var awards = await _awardService.GetAllAwardsAsync();
        return Ok(awards);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAwardById(int id)
    {
        var award = await _awardService.GetAwardByIdAsync(id);
        if (award == null)
            return NotFound(new { message = $"Award with ID {id} not found." });

        return Ok(award);
    }

    [HttpPost]
    public async Task<IActionResult> AddAward([FromBody] AwardRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await _awardService.CreateAwardAsync(request);
        return CreatedAtAction(nameof(GetAwardById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAward(int id, [FromBody] AwardRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _awardService.UpdateAwardAsync(id, request);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAward(int id)
    {
        await _awardService.DeleteAwardAsync(id);
        return NoContent();
    }
}