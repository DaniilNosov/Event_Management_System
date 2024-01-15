using Event_Management_System.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ChartController : ControllerBase
{
    private readonly EventDbContext _dbContext;

    public ChartController(EventDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("ParticipantsData")]
    public async Task<ActionResult<Dictionary<string, int>>> GetParticipantsDataAsync()
    {
        var events = await _dbContext.Events.Include(e => e.Participants)
            .ToListAsync();

        var participantsData = new Dictionary<string, int>();
        foreach (var @event in events)
        {
            var participantsCount = @event.Participants?.Count ?? 0;
            participantsData.Add(@event.Title, participantsCount);
        }

        return Ok(participantsData);
    }
}
