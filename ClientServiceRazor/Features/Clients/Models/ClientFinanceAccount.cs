using System.ComponentModel.DataAnnotations;

namespace ClientServiceRazor.Features.Clients.Models;

public class ClientFinanceAccount
{
    // [Key]
    // public int Id { get; set; }
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
    public int FinanceAccountId { get; set; }
    public FinanceAccount FinanceAccount { get; set; } = null!;
}