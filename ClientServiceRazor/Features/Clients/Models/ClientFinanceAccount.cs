namespace ClientServiceRazor.Features.Clients.Models;

public class ClientFinanceAccount
{
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
    public int FinanceAccountId { get; set; }
    public FinanceAccount FinanceAccount { get; set; } = null!;
}