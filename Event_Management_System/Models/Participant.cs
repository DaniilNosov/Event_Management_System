using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Event_Management_System.Models;

public class Participant
{
    public int Id { get; set; }
    public int? EventId { get; set; }
    [ForeignKey("EventId")]
    public Event? Event { get; set; }
    public string? UserId { get; set; }
    [ForeignKey("UserId")]
    public ApplicationUser? User { get; set; }
}
