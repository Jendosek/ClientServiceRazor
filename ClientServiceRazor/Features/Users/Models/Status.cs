namespace ClientServiceRazor.Features.Users.Models;

public class Status
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    //One Status -> Many Users
    public ICollection<User> Users { get; set; } = new List<User>();
}