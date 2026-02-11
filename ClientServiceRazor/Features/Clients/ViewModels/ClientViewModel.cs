using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClientServiceRazor.Features.Clients.Models;

namespace ClientServiceRazor.Features.Clients.ViewModels;

public class ClientViewModel
{
    [Required]
    [StringLength(50)]
    public string Surname { get; set; } = null!;
    [StringLength(50)]
    public string FirstName { get; set; } = null!;
    [MaxLength(50)]
    public string? Patronymic { get; set; } = null!;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [DataType(DataType.Date)]
    public DateOnly BirthDate { get; set; }
}