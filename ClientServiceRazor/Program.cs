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

// Seed initial data for Roles and Statuses
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Seed Roles
    if (!context.Roles.Any())
    {
        context.Roles.AddRange(
            new Role { Name = "Admin" },
            new Role { Name = "User" },
            new Role { Name = "Manager" }
        );
        context.SaveChanges();
    }
    
    // Seed Statuses
    if (!context.Statuses.Any())
    {
        context.Statuses.AddRange(
            new Status { Name = "Active" },
            new Status { Name = "Inactive" },
            new Status { Name = "Suspended" }
        );
        context.SaveChanges();
    }
}

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


app.Run();