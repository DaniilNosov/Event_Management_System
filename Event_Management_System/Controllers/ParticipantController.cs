using Event_Management_System.Contexts;
using Event_Management_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Event_Management_System.Controllers;
public class ParticipantController : Controller
{
    private readonly EventDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ParticipantController(EventDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Participants
    public async Task<IActionResult> Index()
    {
        var participants = await _context.Participants
            .Include(p => p.Event)
            .Include(p => p.User)
            .ToListAsync();

        return View(participants);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(int eventId)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }

        var currentUser = _userManager.GetUserAsync(User).Result;

        var existingParticipant = _context.Participants
            .FirstOrDefault(p => p.EventId == eventId && p.UserId == currentUser.Id);

        if (existingParticipant != null)
        {
            return RedirectToAction("Details", "Event", new { id = eventId });
        }

        var participant = new Participant
        {
            EventId = eventId,
            UserId = currentUser.Id
        };

        _context.Add(participant);
        _context.SaveChanges();

        return RedirectToAction("Details", "Event", new { id = eventId });
    }



    // GET: Participants/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var participant = await _context.Participants.FindAsync(id);
        if (participant == null)
        {
            return NotFound();
        }

        ViewBag.Events = _context.Events.ToList();
        ViewBag.Users = _context.Users.ToList();

        return View(participant);
    }

    // POST: Participants/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,EventId,UserId")] Participant participant)
    {
        if (id != participant.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(participant);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipantExists(participant.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Events = _context.Events.ToList();
        ViewBag.Users = _context.Users.ToList();

        return View(participant);
    }

    // GET: Participants/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var participant = await _context.Participants
            .Include(p => p.Event)
            .Include(p => p.User)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (participant == null)
        {
            return NotFound();
        }

        return View(participant);
    }

    // POST: Participants/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var participant = await _context.Participants.FindAsync(id);
        _context.Participants.Remove(participant);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ParticipantExists(int id)
    {
        return _context.Participants.Any(e => e.Id == id);
    }
}
