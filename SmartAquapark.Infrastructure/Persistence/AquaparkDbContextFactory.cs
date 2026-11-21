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

        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? throw new InvalidOperationException(
                "Set ConnectionStrings__DefaultConnection before running EF Core commands.");
        optionsBuilder.UseNpgsql(connectionString);

        return new AquaparkDbContext(optionsBuilder.Options);
    }
}
