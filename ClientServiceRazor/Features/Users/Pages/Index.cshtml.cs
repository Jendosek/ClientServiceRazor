using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Users.Models;
using ClientServiceRazor.Features.Users.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ClientServiceRazor.Features.Users.Pages;

public class Index : PageModel
{
    private readonly AppDbContext _dbContext;
    
    public Index(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [BindProperty]
    public UserViewModel NewUser { get; set; } = new();
    public List<User> Users { get; set; } = new();
    public List<Role> Roles { get; set; } = new();
    public List<Status> Statuses { get; set; } = new();
    
    public void OnGet()
    {
        Users = _dbContext.Users
            .Include(u => u.Role)
            .Include(u => u.Status)
            .OrderByDescending(u => u.CreatedAt)
            .ToList();
        Roles = _dbContext.Roles.ToList();
        Statuses = _dbContext.Statuses.ToList();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            Users = _dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.Status)
                .OrderByDescending(u => u.CreatedAt)
                .ToList();
            Roles = _dbContext.Roles.ToList();
            Statuses = _dbContext.Statuses.ToList();
            return Page();
        }

        var user = new User
        {
            Login = NewUser.Login,
            Password = NewUser.Password, // В реальному проекті тут має бути хешування!
            Email = NewUser.Email,
            StatusId = NewUser.StatusId,
            RoleId = NewUser.RoleId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
        return RedirectToPage();
    }
}

