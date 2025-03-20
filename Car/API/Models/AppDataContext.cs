using System;
using Microsoft.EntityFrameworkCore;
using API.Models;

public class AppDataContext : DbContext
{
    public DbSet<Carro> Carros { get; set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
    {
        optionBuilder.UseSqlite("Data Source=bmw.db");
    }
}