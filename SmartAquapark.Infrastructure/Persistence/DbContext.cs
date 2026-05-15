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
    }
}