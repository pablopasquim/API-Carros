using System;
using Microsoft.EntityFrameworkCore;
using API.Models;

public class AppDataContext : DbContext
{
    public DbSet<Carro> Carros { get; set;}
    public DbSet<Modelo> Modelos { get; set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
    {
        optionBuilder.UseSqlite("Data Source=bmw.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        modelBuilder.Entity<Modelo>().HasData(
            new Modelo(){Id = 1, Name = "series 3"},
            new Modelo(){Id = 2, Name = "series 5"},
            new Modelo(){Id = 3, Name = "series X"}
        );
    }
}