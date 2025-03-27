using Microsoft.AspNetCore.Mvc;
using AwesomeFacts.Models;
using AwesomeFacts.Services;

namespace AwesomeFacts.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FactsController : ControllerBase
{
    private readonly IFactService _factService;

    public FactsController(IFactService factService)
    {
        _factService = factService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Fact>>> GetFacts()
    {
        var facts = await _factService.GetAllFactsAsync();
        return Ok(facts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Fact>> GetFact(int id)
    {
        var fact = await _factService.GetFactByIdAsync(id);
        if (fact == null)
            return NotFound();

        return Ok(fact);
    }

    [HttpPost]
    public async Task<ActionResult<Fact>> CreateFact(Fact fact)
    {
        var createdFact = await _factService.CreateFactAsync(fact);
        return CreatedAtAction(nameof(GetFact), new { id = createdFact.Id }, createdFact);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFact(int id, Fact fact)
    {
        var updatedFact = await _factService.UpdateFactAsync(id, fact);
        if (updatedFact == null)
            return NotFound();

        return Ok(updatedFact);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFact(int id)
    {
        var result = await _factService.DeleteFactAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
} 