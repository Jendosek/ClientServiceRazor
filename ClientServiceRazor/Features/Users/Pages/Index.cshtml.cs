using System.Collections.Generic;
using System.Linq;
using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Users.Models;
using ClientServiceRazor.Features.Users.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ClientServiceRazor.Features.Users.Pages;

public class Index : PageModel
{
    private readonly AppDbContext _context;

    public Index(AppDbContext context)
    {
        _context = context;
    }

    public List<User> Users { get; set; } = new();
    public List<Role> Roles { get; set; } = new();
    public List<Status> Statuses { get; set; } = new();
    
    [BindProperty]
    public UserViewModel NewUser { get; set; } = new();

    public void OnGet()
    {
        Users = _context.Users
            .Include(u => u.Role)
            .Include(u => u.Status)
            .ToList();
        Roles = _context.Roles.ToList();
        Statuses = _context.Statuses.ToList();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            Users = _context.Users
                .Include(u => u.Role)
                .Include(u => u.Status)
                .ToList();
            Roles = _context.Roles.ToList();
            Statuses = _context.Statuses.ToList();
            return Page();
        }
        
        var user = new User
        {
            Login = NewUser.Login,
            Password = NewUser.Password,
            Email = NewUser.Email,
            RoleId = NewUser.RoleId,
            StatusId = NewUser.StatusId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        _context.Users.Add(user);
        _context.SaveChanges();
        return RedirectToPage();
    }
}