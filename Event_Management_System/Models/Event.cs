using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Event_Management_System.Models;

public class Event
{
    public int Id { get; set; }
    public string? OrganizerId { get; set; }

    [ForeignKey("OrganizerId")]
    public ApplicationUser? Organizer { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public string Time { get; set; }
    public string Location { get; set; }
    public List<Participant>? Participants { get; set; }
    public int? CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public Category? EventCategory { get; set; }
}
