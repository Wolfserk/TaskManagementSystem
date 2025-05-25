using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Infrastructure.Persistence;
using TaskManagementSystem.Infrastructure.Seed;

namespace TaskManagementSystem.API.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MigrateAndSeedDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        var db = services.GetRequiredService<AppDbContext>();

        try
        {
            logger.LogInformation("Starting database migration...");
            db.Database.Migrate();
            logger.LogInformation("Migration completed successfully");

            logger.LogInformation("Seeding initial data...");
            SeedData.EnsureSeeded(db);
            logger.LogInformation("Data seeding completed");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating or seeding the database");
            throw;
        }

        return app;
    }
}
