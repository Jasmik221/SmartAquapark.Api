using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SmartAquapark.Infrastructure.Persistence;

public class AquaparkDbContextFactory : IDesignTimeDbContextFactory<AquaparkDbContext>
{
    public AquaparkDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AquaparkDbContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=smart_aquapark;Username=postgres;Password=123");

        return new AquaparkDbContext(optionsBuilder.Options);
    }
}