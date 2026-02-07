using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ClientServiceRazor.Features.Clients.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientServiceRazor.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Client> Clients { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Client>()
            .HasIndex(b => b.Email)
            .IsUnique();

        var assembly = Assembly.GetExecutingAssembly();
        var appNamespace = "ClientServiceRazor";

        var types = assembly
            .GetTypes()
            .Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                (t.IsPublic || t.IsNestedPublic) &&                            
                t.Namespace != null &&
                t.Namespace.StartsWith(appNamespace, StringComparison.Ordinal) &&
                !t.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false) && 
                t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Any(p => string.Equals(p.Name, "Id", StringComparison.OrdinalIgnoreCase))
            );

        foreach (var t in types)
        {
            if (modelBuilder.Model.FindEntityType(t) is null)
            {
                modelBuilder.Entity(t);
            }
        }
    }
}