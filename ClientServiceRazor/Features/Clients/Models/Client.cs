using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientServiceRazor.Features.Clients.Models;

public class Client
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    [MinLength(1)]
    [Display(Name = "Client's surname")]
    [DataType(DataType.Text)]
    public string Surname { get; set; } = null!;
    [StringLength(50)]
    [DataType(DataType.Text)]
    public string FirstName { get; set; } = null!;
    [MaxLength(50)]
    [DataType(DataType.Text)]
    public string? Patronymic { get; set; } = null!;
    [MaxLength(100)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
    public DateOnly BirthDate { get; set; }
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    //One Client -> Many Phones
    public ICollection<Phone> Phones { get; set; } = new List<Phone>();

    //One Client -> One Address
    public Address? Address { get; set; }
}