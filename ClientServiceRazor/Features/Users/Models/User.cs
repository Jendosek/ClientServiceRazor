namespace ClientServiceRazor.Features.Users.Models;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
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