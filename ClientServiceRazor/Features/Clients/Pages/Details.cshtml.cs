using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Clients.Models;
using ClientServiceRazor.Features.Clients.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
    
    public void OnGet(int id)
    {
        NewClient = _dbContext.Clients.Where(client => client.Id == id)
            .Select(client => new ClientViewModel
        {
            Surname = client.Surname,
            FirstName = client.FirstName,
            Patronymic = client.Patronymic,
            Email = client.Email,
            BirthDate = client.BirthDate
        })
            .FirstOrDefault()  ?? new ClientViewModel();
        Console.WriteLine(NewClient);
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