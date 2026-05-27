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
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Wristband> Wristbands => Set<Wristband>();
    public DbSet<User> Users => Set<User>();

    public DbSet<ZoneVisit> ZoneVisits => Set<ZoneVisit>();

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
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("tickets");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id");

            entity.Property(x => x.VerificationCode)
                .HasColumnName("verification_code")
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(x => x.VerificationCode)
                .IsUnique();

            entity.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.Property(x => x.ValidUntil)
                .HasColumnName("valid_until")
                .IsRequired();

            entity.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .IsRequired();

            entity.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Price)
                .HasColumnName("price")
                .HasColumnType("numeric(10,2)")
                .IsRequired();

            entity.Property(x => x.NumberOfPeople)
                .HasColumnName("number_of_people")
                .HasDefaultValue(1)
                .IsRequired();
        });
        modelBuilder.Entity<Wristband>(entity =>
        {
            entity.ToTable("wristbands");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id");

            entity.Property(x => x.QrCode)
                .HasColumnName("qr_code")
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(x => x.QrCode)
                .IsUnique();

            entity.Property(x => x.ActivatedAt)
                .HasColumnName("activated_at")
                .IsRequired();

            entity.Property(x => x.ExitTime)
                .HasColumnName("exit_time");

            entity.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .IsRequired();

            entity.Property(x => x.TicketId)
                .HasColumnName("ticket_id")
                .IsRequired();

            entity.HasOne(x => x.Ticket)
                .WithMany(x => x.Wristbands)
                .HasForeignKey(x => x.TicketId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ZoneVisit>(entity =>
        {
            entity.ToTable("zone_visits");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id");

            entity.Property(x => x.WristbandId)
                .HasColumnName("wristband_id")
                .IsRequired();

            entity.Property(x => x.ZoneId)
                .HasColumnName("zone_id")
                .IsRequired();

            entity.Property(x => x.EnteredAt)
                .HasColumnName("entered_at")
                .IsRequired();

            entity.Property(x => x.ExitedAt)
                .HasColumnName("exited_at");

            entity.HasOne(x => x.Wristband)
                .WithMany(x => x.ZoneVisits)
                .HasForeignKey(x => x.WristbandId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Zone)
                .WithMany(x => x.ZoneVisits)
                .HasForeignKey(x => x.ZoneId)
                .OnDelete(DeleteBehavior.Cascade);
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