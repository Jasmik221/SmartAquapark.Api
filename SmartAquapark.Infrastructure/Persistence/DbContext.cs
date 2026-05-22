using Microsoft.EntityFrameworkCore;
using SmartAquapark.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace SmartAquapark.Infrastructure.Persistence;

public class AquaparkDbContext : DbContext
{
    public AquaparkDbContext(DbContextOptions<AquaparkDbContext> options)
        : base(options)
    {
    }

    public DbSet<Zone> Zones => Set<Zone>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Zone>(entity =>
        {
            entity.ToTable("strefa");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id");

            entity.Property(x => x.Name)
                .HasColumnName("nazwa")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Description)
                .HasColumnName("opis");

            entity.Property(x => x.CapacityLimit)
                .HasColumnName("limit_pojemnosci")
                .IsRequired();

            entity.Property(x => x.CurrentPeopleCount)
                .HasColumnName("aktualna_liczba_osob")
                .HasDefaultValue(0);

            entity.Property(x => x.IsActive)
                .HasColumnName("aktywna")
                .HasDefaultValue(true);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id");

            entity.Property(x => x.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.Email)
                .HasColumnName("email")
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(x => x.Email)
                .IsUnique();

            entity.Property(x => x.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired();

            entity.Property(x => x.Role)
                .HasColumnName("role")
                .HasConversion<string>()
                .IsRequired();
        });
    }
}