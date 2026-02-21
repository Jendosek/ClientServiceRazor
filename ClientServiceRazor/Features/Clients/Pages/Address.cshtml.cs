using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Clients.Models;
using ClientServiceRazor.Features.Clients.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ClientServiceRazor.Features.Clients.Pages;

public class Address : PageModel
{
    private readonly AppDbContext _dbContext;
    
    public Address(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [BindProperty]
    public AddressViewModel AddressData { get; set; } = new();
    
    public bool HasAddress { get; set; }
    
    public int ClientId { get; set; }
    
    public void OnGet(int id)
    {
        ClientId = id;
        var client = _dbContext.Clients
            .Include(c => c.Address)
            .FirstOrDefault(c => c.Id == id);
        
        if (client?.Address != null)
        {
            HasAddress = true;
            AddressData = new AddressViewModel
            {
                Country = client.Address.Country,
                Region = client.Address.Region,
                Area = client.Address.Area,
                City = client.Address.City,
                Street = client.Address.Street,
                Building = client.Address.Building,
                Apartment = client.Address.Apartment,
                Entrance = client.Address.Entrance,
                Room = client.Address.Room
            };
        }
        else
        {
            HasAddress = false;
        }
    }

    public IActionResult OnPost(int id)
    {
        ClientId = id;
        
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var client = _dbContext.Clients
            .Include(c => c.Address)
            .FirstOrDefault(c => c.Id == id);
        
        if (client == null)
        {
            return NotFound();
        }

        HasAddress = client.Address != null;

        if (client.Address != null)
        {
            client.Address.Country = AddressData.Country;
            client.Address.Region = AddressData.Region;
            client.Address.Area = AddressData.Area;
            client.Address.City = AddressData.City;
            client.Address.Street = AddressData.Street;
            client.Address.Building = AddressData.Building;
            client.Address.Apartment = AddressData.Apartment;
            client.Address.Entrance = AddressData.Entrance;
            client.Address.Room = AddressData.Room;
            client.Address.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            client.Address = new Models.Address
            {
                Country = AddressData.Country,
                Region = AddressData.Region,
                Area = AddressData.Area,
                City = AddressData.City,
                Street = AddressData.Street,
                Building = AddressData.Building,
                Apartment = AddressData.Apartment,
                Entrance = AddressData.Entrance,
                Room = AddressData.Room,
                ClientId = client.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        _dbContext.SaveChanges();

        return RedirectToPage("/Features/Clients/Pages/Details", new { id });
    }

    public IActionResult OnPostClearAddress(int id)
    {
        ClientId = id;
        var client = _dbContext.Clients
            .Include(c => c.Address)
            .FirstOrDefault(c => c.Id == id);
        
        if (client == null)
        {
            return NotFound();
        }

        if (client.Address != null)
        {
            client.Address = null;
            _dbContext.SaveChanges();
        }

        return RedirectToPage("/Features/Clients/Pages/Address", new { id });
    }
}