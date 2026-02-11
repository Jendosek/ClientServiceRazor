using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Clients.Models;
using ClientServiceRazor.Features.Clients.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClientServiceRazor.Features.Clients.Pages;

public class Index : PageModel
{
    private readonly AppDbContext _dbContext;
    
    public Index(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [BindProperty]
    public ClientViewModel NewClient { get; set; } = new();
    public List<Client> Clients { get;set; } = new();
    
    public void OnGet()
    {
        // Clients = _dbContext.Clients.ToList();
        Clients = _dbContext.Clients.OrderByDescending(c => c.CreatedAt).ToList();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            Clients = _dbContext.Clients.OrderByDescending(c => c.CreatedAt).ToList();
            return Page();
        }

        var client = new Client
        {
            Surname = NewClient.Surname,
            FirstName = NewClient.FirstName,
            Patronymic = NewClient.Patronymic,
            Email = NewClient.Email,
            BirthDate = NewClient.BirthDate,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        _dbContext.Clients.Add(client);
        _dbContext.SaveChanges();
        return RedirectToPage();
    }
}