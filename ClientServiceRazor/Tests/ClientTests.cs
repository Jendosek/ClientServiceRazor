using System;
using System.Linq;
using System.Collections.Generic;
using ClientServiceRazor.Data;
using ClientServiceRazor.Features.Clients.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClientServiceRazor.Tests;

public class ClientTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public void CreateClient_SavesAndReadsBack()
    {
        using var context = GetDbContext();
        var now = DateTime.UtcNow;
        var client = new Client
        {
            FirstName = "John",
            Surname = "Doe",
            Email = "john.doe@example.com",
            Patronymic = "Patron",
            BirthDate = DateOnly.FromDateTime(DateTime.Now),
            CreatedAt = now,
            UpdatedAt = now
        };

        context.Clients.Add(client);
        context.SaveChanges();

        var fromDb = context.Clients.First(c => c.Id == client.Id);

        Assert.Equal(client.Surname, fromDb.Surname);
        Assert.Equal(client.FirstName, fromDb.FirstName);
        Assert.Equal(client.Email, fromDb.Email);
        Assert.Equal(client.BirthDate, fromDb.BirthDate);
    }

    [Fact]
    public void GetAllClients_ReturnsAllSavedClients()
    {
        using var context = GetDbContext();
        var now = DateTime.UtcNow;
        var clients = new List<Client>
        {
            new Client { FirstName = "A", Surname = "Alpha", Email = "a@example.com", BirthDate = DateOnly.FromDateTime(DateTime.Now), CreatedAt = now, UpdatedAt = now },
            new Client { FirstName = "B", Surname = "Beta", Email = "b@example.com", BirthDate = DateOnly.FromDateTime(DateTime.Now), CreatedAt = now, UpdatedAt = now },
            new Client { FirstName = "C", Surname = "Gamma", Email = "c@example.com", BirthDate = DateOnly.FromDateTime(DateTime.Now), CreatedAt = now, UpdatedAt = now }
        };

        context.Clients.AddRange(clients);
        context.SaveChanges();

        var list = context.Clients.ToList();

        Assert.Equal(3, list.Count);
        Assert.Contains(list, c => c.Surname == "Alpha");
        Assert.Contains(list, c => c.Surname == "Beta");
        Assert.Contains(list, c => c.Surname == "Gamma");
    }

    [Fact]
    public void DeleteClient_RemovesFromDatabase()
    {
        using var context = GetDbContext();
        var now = DateTime.UtcNow;
        var client = new Client
        {
            FirstName = "ToDelete",
            Surname = "Remove",
            Email = "remove@example.com",
            BirthDate = DateOnly.FromDateTime(DateTime.Now),
            CreatedAt = now,
            UpdatedAt = now
        };

        context.Clients.Add(client);
        context.SaveChanges();

        // Remove and save
        context.Clients.Remove(client);
        context.SaveChanges();

        Assert.False(context.Clients.Any());
    }

    [Fact]
    public void UpdateClient_ChangesPersisted()
    {
        using var context = GetDbContext();
        var now = DateTime.UtcNow;
        var client = new Client
        {
            FirstName = "Old",
            Surname = "Surname",
            Email = "old@example.com",
            BirthDate = DateOnly.FromDateTime(DateTime.Now),
            CreatedAt = now,
            UpdatedAt = now
        };

        context.Clients.Add(client);
        context.SaveChanges();

        client.Surname = "NewSurname";
        client.Email = "new@example.com";
        client.UpdatedAt = DateTime.UtcNow;
        context.SaveChanges();

        var fromDb = context.Clients.Single(c => c.Id == client.Id);
        Assert.Equal("NewSurname", fromDb.Surname);
        Assert.Equal("new@example.com", fromDb.Email);
    }

    [Fact]
    public void Client_WithPhones_SavesAndIncludesPhones()
    {
        using var context = GetDbContext();
        var now = DateTime.UtcNow;
        var client = new Client
        {
            FirstName = "Phone",
            Surname = "Owner",
            Email = "phone.owner@example.com",
            BirthDate = DateOnly.FromDateTime(DateTime.Now),
            CreatedAt = now,
            UpdatedAt = now
        };

        var phone1 = new Phone { Number = "+380501234567", CountryCode = CountryCode.UA, CreatedAt = now, UpdatedAt = now };
        var phone2 = new Phone { Number = "+380501234568", CountryCode = CountryCode.UA, CreatedAt = now, UpdatedAt = now };

        client.Phones.Add(phone1);
        client.Phones.Add(phone2);

        context.Clients.Add(client);
        context.SaveChanges();

        var fromDb = context.Clients
            .Include(c => c.Phones)
            .Single(c => c.Id == client.Id);

        Assert.Equal(2, fromDb.Phones.Count);
        Assert.Contains(fromDb.Phones, p => p.Number == "+380501234567");
        Assert.Contains(fromDb.Phones, p => p.Number == "+380501234568");

        foreach (var p in fromDb.Phones)
        {
            Assert.Equal(client.Id, p.ClientId);
        }
    }
}