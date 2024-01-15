using Event_Management_System.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Event_Management_System.Contexts;

public class EventDbContext : IdentityDbContext<ApplicationUser>
{
    public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Participant> Participants { get; set; }
}
