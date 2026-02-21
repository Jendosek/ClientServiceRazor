using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Clients.Models;
using ClientServiceRazor.Features.Clients.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ClientServiceRazor.Features.Clients.Pages;

public class Details : PageModel
{
    private readonly AppDbContext _dbContext;
    
    public Details(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [BindProperty]
    public ClientViewModel NewClient { get; set; } = new();

    public int ClientId { get; set; }

    public AddressViewModel AddressData { get; set; } = new();

    public bool HasAddress { get; set; }

    public void OnGet(int id)
    {
        ClientId = id;
        var client = _dbContext.Clients
            .Include(c => c.Address)
            .FirstOrDefault(c => c.Id == id);

        if (client == null)
        {
            NewClient = new ClientViewModel();
            HasAddress = false;
            return;
        }

        NewClient = new ClientViewModel
        {
            Surname = client.Surname,
            FirstName = client.FirstName,
            Patronymic = client.Patronymic,
            Email = client.Email,
            BirthDate = client.BirthDate
        };

        HasAddress = client.Address != null;
        if (client.Address != null)
        {
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
    }

    public IActionResult OnPost(int id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var client = _dbContext.Clients.FirstOrDefault(c => c.Id == id);
        
        if (client == null)
        {
            return NotFound();
        }

        client.Surname = NewClient.Surname;
        client.FirstName = NewClient.FirstName;
        client.Patronymic = NewClient.Patronymic;
        client.Email = NewClient.Email;
        client.BirthDate = NewClient.BirthDate;

        _dbContext.SaveChanges();

        return RedirectToPage("/Features/Clients/Pages/Index");
    }
}