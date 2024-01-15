using Microsoft.AspNetCore.Identity;

namespace Event_Management_System.Models;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email {  get; set; }
    public string? PhoneNumber { get; set; }
    public List<Event>? Events { get; set; }
    public List<Participant>? Participants { get; set; }
}
