using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SmartAquapark.Infrastructure.Persistence;

namespace SmartAquapark.Api.Health;

public sealed class DatabaseHealthCheck(IServiceScopeFactory scopeFactory) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        using var scope = scopeFactory.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<AquaparkDbContext>().Database;
        if (!await database.CanConnectAsync(cancellationToken))
            return HealthCheckResult.Unhealthy("Database is unavailable.");

        var pending = await database.GetPendingMigrationsAsync(cancellationToken);
        return pending.Any()
            ? HealthCheckResult.Unhealthy("Database migrations are pending.")
            : HealthCheckResult.Healthy();
    }
}
