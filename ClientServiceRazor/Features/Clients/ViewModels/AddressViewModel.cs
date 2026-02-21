using System.ComponentModel.DataAnnotations;

namespace ClientServiceRazor.Features.Clients.ViewModels;

public class AddressViewModel
{
    [Required]
    [MaxLength(100)]
    public string Country { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Region { get; set; } = null!;

    [MaxLength(100)]
    public string? Area { get; set; }

    [Required]
    [MaxLength(100)]
    public string City { get; set; } = null!;

    [Required]
    [MaxLength(150)]
    public string Street { get; set; } = null!;

    [Required]
    [MaxLength(20)]
    public string Building { get; set; } = null!;

    [MaxLength(20)]
    public string? Apartment { get; set; }

    [MaxLength(10)]
    public string? Entrance { get; set; }

    [MaxLength(20)]
    public string? Room { get; set; }
}

