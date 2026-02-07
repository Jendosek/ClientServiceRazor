using System.ComponentModel.DataAnnotations;

namespace ClientServiceRazor.Features.Users.Models;

public class Role
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    //One Role -> Many Users
    public ICollection<User> Users { get; set; } = new List<User>();
}