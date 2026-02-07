using System.ComponentModel.DataAnnotations;
using ClientServiceRazor.Features.Clients.Models;

namespace ClientServiceRazor.Features.Users.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Login { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string Password { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    //Many Users -> One Status
    public int StatusId { get; set; }
    public Status Status { get; set; } = null!;

    //Many Users -> One Role
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
}