using Event_Management_System.Contexts;
using Event_Management_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Event_Management_System.Controllers;
public class EventController : Controller
{
    private readonly EventDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public EventController(EventDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        this._userManager = userManager;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        var events = _context.Events
            .Include(e => e.Organizer)
            .Include(e => e.EventCategory)
            .ToList();

        return View(events);
    }

    public IActionResult Create()
    {
        var categories = _context.Categories.ToList();
        ViewData["Categories"] = categories.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Event @event)
    {
        if (ModelState.IsValid)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            @event.OrganizerId = currentUser.Id;

            _context.Events.Add(@event);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        var categories = _context.Categories.ToList();
        ViewData["Categories"] = categories.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
        return View(@event);
    }


    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var @event = _context.Events
            .Include(e => e.Organizer)
            .Include(e => e.EventCategory)
            .FirstOrDefault(e => e.Id == id);

        if (@event == null)
        {
            return NotFound();
        }

        ViewBag.Organizers = _context.Users.ToList();

        // Convert the list of Category objects to a list of SelectListItem
        ViewBag.Categories = _context.Categories
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
            .ToList();

        return View(@event);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Event @event)
    {
        if (id != @event.Id)
        {
            return NotFound();
        }
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (ModelState.IsValid)
        {
            try
            {
                @event.OrganizerId = userId;
                _context.Events.Update(@event);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(@event.Id))
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

        ViewBag.Organizers = _context.Users.ToList();

        // Convert the list of Category objects to a list of SelectListItem
        ViewBag.Categories = _context.Categories
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
            .ToList();

        return View(@event);
    }


    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var @event = _context.Events
            .Include(e => e.Organizer)
            .Include(e => e.EventCategory)
            .FirstOrDefault(e => e.Id == id);

        if (@event == null)
        {
            return NotFound();
        }

        return View(@event);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var @event = _context.Events.Find(id);
        _context.Events.Remove(@event);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var @event = _context.Events
            .Include(e => e.Organizer)
            .Include(e => e.EventCategory)
            .Include(e => e.Participants)
            .FirstOrDefault(e => e.Id == id);

        if (@event == null)
        {
            return NotFound();
        }

        var currentUser = _userManager.GetUserAsync(User).Result;
        ViewBag.HasParticipated = _context.Participants.Any(p => p.EventId == id && p.UserId == currentUser.Id);

        return View(@event);
    }

    private bool EventExists(int id)
    {
        return _context.Events.Any(e => e.Id == id);
    }
}
