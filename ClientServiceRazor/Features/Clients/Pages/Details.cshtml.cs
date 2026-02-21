using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Clients.Models;
using ClientServiceRazor.Features.Clients.Services;
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
    
    [BindProperty(SupportsGet = false)]
    public ClientViewModel NewClient { get; set; } = new();

    [BindProperty]
    public PhoneViewModel NewPhone { get; set; } = new();

    public int ClientId { get; set; }

    public AddressViewModel AddressData { get; set; } = new();

    public bool HasAddress { get; set; }

    public bool ShowPhoneForm { get; set; }

    public bool ShowEditPhoneForm { get; set; }

    public int EditingPhoneId { get; set; }

    public List<Phone> Phones { get; set; } = new();

    public void OnGet(int id)
    {
        ClientId = id;
        LoadClientData(id);
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

    public IActionResult OnPostShowPhoneForm(int id)
    {
        foreach (var key in ModelState.Keys.Where(k => k.StartsWith("NewClient")).ToList())
        {
            ModelState.Remove(key);
        }
        
        ClientId = id;
        ShowPhoneForm = true;
        LoadClientData(id);
        return Page();
    }

    public IActionResult OnPostAddPhone(int id)
    {
        foreach (var key in ModelState.Keys.Where(k => k.StartsWith("NewClient")).ToList())
        {
            ModelState.Remove(key);
        }
        
        if (string.IsNullOrWhiteSpace(NewPhone?.Number))
        {
            ClientId = id;
            ShowPhoneForm = true;
            LoadClientData(id);
            return Page();
        }

        var formattedNumber = PhoneMaskService.FormatPhoneNumber(NewPhone.Number, NewPhone.CountryCode);
        
        var phone = new Phone
        {
            Number = formattedNumber,
            CountryCode = NewPhone.CountryCode,
            ClientId = id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Phones.Add(phone);
        _dbContext.SaveChanges();

        // Clear the form
        NewPhone = new PhoneViewModel();
        ShowPhoneForm = false;

        return RedirectToPage("/Features/Clients/Pages/Details", new { id });
    }

    public IActionResult OnPostDeletePhone(int id, int phoneId)
    {
        foreach (var key in ModelState.Keys.Where(k => k.StartsWith("NewClient")).ToList())
        {
            ModelState.Remove(key);
        }
        
        var phone = _dbContext.Phones.FirstOrDefault(p => p.Id == phoneId);
        
        if (phone == null)
        {
            return NotFound();
        }

        _dbContext.Phones.Remove(phone);
        _dbContext.SaveChanges();

        return RedirectToPage("/Features/Clients/Pages/Details", new { id });
    }

    public IActionResult OnPostEditPhoneForm(int id, int phoneId)
    {
        foreach (var key in ModelState.Keys.Where(k => k.StartsWith("NewClient")).ToList())
        {
            ModelState.Remove(key);
        }

        ClientId = id;
        EditingPhoneId = phoneId;
        ShowEditPhoneForm = true;

        var phone = _dbContext.Phones.FirstOrDefault(p => p.Id == phoneId);
        if (phone != null)
        {
            NewPhone = new PhoneViewModel
            {
                Number = phone.Number,
                CountryCode = phone.CountryCode
            };
        }

        LoadClientData(id);
        return Page();
    }

    public IActionResult OnPostUpdatePhone(int id, int phoneId)
    {
        foreach (var key in ModelState.Keys.Where(k => k.StartsWith("NewClient")).ToList())
        {
            ModelState.Remove(key);
        }

        if (string.IsNullOrWhiteSpace(NewPhone?.Number))
        {
            ClientId = id;
            EditingPhoneId = phoneId;
            ShowEditPhoneForm = true;
            LoadClientData(id);
            return Page();
        }

        var phone = _dbContext.Phones.FirstOrDefault(p => p.Id == phoneId && p.ClientId == id);

        if (phone == null)
        {
            return NotFound();
        }

        var formattedNumber = PhoneMaskService.FormatPhoneNumber(NewPhone.Number, NewPhone.CountryCode);
        
        phone.Number = formattedNumber;
        phone.CountryCode = NewPhone.CountryCode;
        phone.UpdatedAt = DateTime.UtcNow;

        _dbContext.SaveChanges();

        NewPhone = new PhoneViewModel();
        ShowEditPhoneForm = false;

        return RedirectToPage("/Features/Clients/Pages/Details", new { id });
    }

    private void LoadClientData(int id)
    {
        var client = _dbContext.Clients
            .Include(c => c.Address)
            .FirstOrDefault(c => c.Id == id);

        if (client == null)
        {
            NewClient = new ClientViewModel();
            HasAddress = false;
            Phones = new List<Phone>();
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

        // Load phones separately
        Phones = _dbContext.Phones
            .Where(p => p.ClientId == id)
            .ToList();
    }

    public string GetCountryName(CountryCode countryCode)
    {
        return countryCode switch
        {
            CountryCode.UA => "Ukraine",
            CountryCode.US => "USA",
            CountryCode.GB => "Great Britain",
            CountryCode.DE => "Germany",
            CountryCode.FR => "France",
            _ => "Unknown"
        };
    }
}