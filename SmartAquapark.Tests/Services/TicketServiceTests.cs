using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using SmartAquapark.Application.DTOs;
using SmartAquapark.Domain.Enums;
using SmartAquapark.Infrastructure.Persistence;
using SmartAquapark.Infrastructure.Services;

namespace SmartAquapark.Tests.Services;

public class TicketServiceTests
{
    private static AquaparkDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AquaparkDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AquaparkDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateTicketWithNewStatus()
    {
        // Arrange
        var context = CreateDbContext();

        var service = new TicketService(context);

        var dto = new CreateTicketDto
        {
            Name = "Bilet testowy",
            Price = 50,
            NumberOfPeople = 1,
            ValidUntil = DateTime.UtcNow.AddDays(1)
        };

        // Act
        var result = await service.CreateAsync(dto);

        // Assert
        Assert.NotNull(result);

        Assert.Equal("Bilet testowy", result.Name);

        Assert.Equal(50, result.Price);

        Assert.Equal(1, result.NumberOfPeople);

        Assert.Equal(TicketStatus.New, result.Status);

        Assert.False(string.IsNullOrWhiteSpace(result.VerificationCode));
    }

    [Fact]
    public async Task ActivateAsync_ShouldChangeStatusToActive()
    {
        // Arrange
        var context = CreateDbContext();

        var service = new TicketService(context);

        var dto = new CreateTicketDto
        {
            Name = "Bilet VIP",
            Price = 100,
            NumberOfPeople = 2,
            ValidUntil = DateTime.UtcNow.AddDays(1)
        };

        var createdTicket = await service.CreateAsync(dto);

        // Act
        var result = await service.ActivateAsync(createdTicket.Id);

        var activatedTicket = await service.GetByIdAsync(createdTicket.Id);

        // Assert
        Assert.True(result);

        Assert.NotNull(activatedTicket);

        Assert.Equal(TicketStatus.Active, activatedTicket!.Status);
    }
}