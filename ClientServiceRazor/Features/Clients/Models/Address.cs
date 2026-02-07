using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientServiceRazor.Features.Clients.Models;

public class Address
{
    public int Id { get; set; }

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

    [Column(TypeName = "timestamptz")]
    public DateTime CreatedAt { get; set; }

    [Column(TypeName = "timestamptz")]
    public DateTime UpdatedAt { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
}