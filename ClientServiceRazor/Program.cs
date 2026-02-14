using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Users.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddRazorPages();

builder.Services.AddRazorPages(options =>
    {
        options.RootDirectory = "/";
        options.Conventions.AddFolderRouteModelConvention(
            "/Features",
            model =>
            {
                foreach (var selector in model.Selectors)
                {
                    selector.AttributeRouteModel.Template = 
                        selector.AttributeRouteModel.Template
                            .Replace("Features/", "")
                            .Replace("/Pages", "");
                }
            });
    }
);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.MapGet("/",
    context =>
    {
        context.Response.Redirect("/Clients");
        return Task.CompletedTask;
    }
);

// Seed data for Roles and Statuses
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Ensure database is created
    context.Database.EnsureCreated();
    
    // Seed Roles if they don't exist
    if (!context.Roles.Any())
    {
        var roles = new[]
        {
            new Role { Name = "Admin" },
            new Role { Name = "User" },
            new Role { Name = "Manager" }
        };
        context.Roles.AddRange(roles);
        context.SaveChanges();
    }
    
    // Seed Statuses if they don't exist
    if (!context.Statuses.Any())
    {
        var statuses = new[]
        {
            new Status { Name = "Active" },
            new Status { Name = "Inactive" },
            new Status { Name = "Suspended" }
        };
        context.Statuses.AddRange(statuses);
        context.SaveChanges();
    }
}

app.Run();