using System;
using System.Linq;
using System.Collections.Generic;
using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Users.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClientServiceRazor.Tests;

public class UserTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void CreateUser_SavesAndReadsBack()
    {
        using var context = GetDbContext();
        var now = DateTime.UtcNow;

        var role = new Role { Name = "Admin" };
        var status = new Status { Name = "Active" };
        context.Add(role);
        context.Add(status);
        context.SaveChanges();

        var user = new User
        {
            Login = "jdoe",
            Password = "secret",
            Email = "jdoe@example.com",
            RoleId = role.Id,
            StatusId = status.Id,
            CreatedAt = now,
            UpdatedAt = now
        };

        context.Add(user);
        context.SaveChanges();

        var fromDb = context.Set<User>().First(u => u.Id == user.Id);

        Assert.Equal(user.Login, fromDb.Login);
        Assert.Equal(user.Email, fromDb.Email);
        Assert.Equal(user.RoleId, fromDb.RoleId);
        Assert.Equal(user.StatusId, fromDb.StatusId);
    }

    [Fact]
    public void GetAllUsers_ReturnsAllSavedUsers()
    {
        using var context = GetDbContext();
        var now = DateTime.UtcNow;

        var role = new Role { Name = "R" };
        var status = new Status { Name = "S" };
        context.Add(role);
        context.Add(status);
        context.SaveChanges();

        var users = new List<User>
        {
            new User { Login = "u1", Password = "p", Email = "u1@example.com", RoleId = role.Id, StatusId = status.Id, CreatedAt = now, UpdatedAt = now },
            new User { Login = "u2", Password = "p", Email = "u2@example.com", RoleId = role.Id, StatusId = status.Id, CreatedAt = now, UpdatedAt = now },
            new User { Login = "u3", Password = "p", Email = "u3@example.com", RoleId = role.Id, StatusId = status.Id, CreatedAt = now, UpdatedAt = now }
        };

        context.AddRange(users);
        context.SaveChanges();

        var list = context.Set<User>().ToList();

        Assert.Equal(3, list.Count);
        Assert.Contains(list, u => u.Login == "u1");
        Assert.Contains(list, u => u.Login == "u2");
        Assert.Contains(list, u => u.Login == "u3");
    }

    [Fact]
    public void DeleteUser_RemovesFromDatabase()
    {
        using var context = GetDbContext();
        var now = DateTime.UtcNow;

        var role = new Role { Name = "RoleDel" };
        var status = new Status { Name = "StatusDel" };
        context.Add(role);
        context.Add(status);
        context.SaveChanges();

        var user = new User { Login = "todel", Password = "p", Email = "todel@example.com", RoleId = role.Id, StatusId = status.Id, CreatedAt = now, UpdatedAt = now };
        context.Add(user);
        context.SaveChanges();

        context.Set<User>().Remove(user);
        context.SaveChanges();

        Assert.False(context.Set<User>().Any());
    }

    [Fact]
    public void UpdateUser_ChangesPersisted()
    {
        using var context = GetDbContext();
        var now = DateTime.UtcNow;

        var role = new Role { Name = "RoleUp" };
        var status = new Status { Name = "StatusUp" };
        context.Add(role);
        context.Add(status);
        context.SaveChanges();

        var user = new User { Login = "oldlogin", Password = "p", Email = "old@example.com", RoleId = role.Id, StatusId = status.Id, CreatedAt = now, UpdatedAt = now };
        context.Add(user);
        context.SaveChanges();

        user.Login = "newlogin";
        user.Email = "new@example.com";
        user.UpdatedAt = DateTime.UtcNow;
        context.SaveChanges();

        var fromDb = context.Set<User>().Single(u => u.Id == user.Id);
        Assert.Equal("newlogin", fromDb.Login);
        Assert.Equal("new@example.com", fromDb.Email);
    }

    [Fact]
    public void User_WithRoleAndStatus_SavesAndIncludesRelations()
    {
        using var context = GetDbContext();
        var now = DateTime.UtcNow;

        var role = new Role { Name = "Member" };
        var status = new Status { Name = "Pending" };
        context.Add(role);
        context.Add(status);
        context.SaveChanges();

        var user = new User { Login = "reluser", Password = "p", Email = "rel@example.com", RoleId = role.Id, StatusId = status.Id, CreatedAt = now, UpdatedAt = now };
        context.Add(user);
        context.SaveChanges();

        var fromDb = context.Set<User>()
            .Include(u => u.Role)
            .Include(u => u.Status)
            .Single(u => u.Id == user.Id);

        Assert.Equal(role.Id, fromDb.Role.Id);
        Assert.Equal(role.Name, fromDb.Role.Name);
        Assert.Equal(status.Id, fromDb.Status.Id);
        Assert.Equal(status.Name, fromDb.Status.Name);
    }
}