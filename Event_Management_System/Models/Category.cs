using System.ComponentModel.DataAnnotations;

namespace Event_Management_System.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Event>? Events { get; set; }
}
