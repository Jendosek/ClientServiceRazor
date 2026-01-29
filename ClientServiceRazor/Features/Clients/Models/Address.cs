namespace ClientServiceRazor.Features.Clients.Models;

public class Address
{
    public  int Id { get; set; }
    public string Country { get; set; } = null!;
    public string Region { get; set; } = null!;
    public string Area { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string Building { get; set; } = null!;
    public string Apartment { get; set; } = null!;
    public string Entrance { get; set; } = null!;
    public string Room { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    //One Address -> One Client
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
    
}