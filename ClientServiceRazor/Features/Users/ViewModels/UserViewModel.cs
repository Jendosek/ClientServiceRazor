using System.ComponentModel.DataAnnotations;

namespace ClientServiceRazor.Features.Users.ViewModels;

public class UserViewModel
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Login { get; set; } = null!;

    [Required]
    [StringLength(255, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = null!;

    [Required]
    public int StatusId { get; set; }

    [Required]
    public int RoleId { get; set; }
}
