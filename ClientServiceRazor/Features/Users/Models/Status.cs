using System.ComponentModel.DataAnnotations;

namespace ClientServiceRazor.Features.Users.Models;

public class Status
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    //One Status -> Many Users
    public ICollection<User> Users { get; set; } = new List<User>();
}