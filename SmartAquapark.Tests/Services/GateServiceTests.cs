using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using SmartAquapark.Application.DTOs;
using SmartAquapark.Domain.Entities;
using SmartAquapark.Domain.Enums;
using SmartAquapark.Infrastructure.Persistence;
using SmartAquapark.Infrastructure.Services;

namespace SmartAquapark.Tests.Services;

public class GateServiceTests
{
    private static AquaparkDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AquaparkDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AquaparkDbContext(options);
    }

    [Fact]
    public async Task ScanAsync_ShouldGrantAccess_WhenEverythingIsValid()
    {
        // Arrange
        var context = CreateDbContext();

        var zone = new Zone
        {
            Name = "VIP Zone",
            Description = "Test",
            CapacityLimit = 10,
            CurrentPeopleCount = 0,
            IsActive = true
        };

        context.Zones.Add(zone);

        var ticket = new Ticket
        {
            VerificationCode = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.UtcNow,
            ValidUntil = DateTime.UtcNow.AddDays(1),
            Status = TicketStatus.Active,
            Name = "VIP Ticket",
            Price = 100,
            NumberOfPeople = 1
        };

        context.Tickets.Add(ticket);

        await context.SaveChangesAsync();

        var wristband = new Wristband
        {
            QrCode = "WB-TEST",
            ActivatedAt = DateTime.UtcNow,
            Status = WristbandStatus.Active,
            TicketId = ticket.Id
        };

        context.Wristbands.Add(wristband);

        await context.SaveChangesAsync();

        var service = new GateService(context);

        var dto = new GateScanDto
        {
            QrCode = "WB-TEST",
            ZoneId = zone.Id
        };

        // Act
        var result = await service.ScanAsync(dto);

        // Assert
        Assert.True(result.AccessGranted);

        Assert.Equal("Access granted.", result.Message);

        Assert.Equal(1, zone.CurrentPeopleCount);
    }
}