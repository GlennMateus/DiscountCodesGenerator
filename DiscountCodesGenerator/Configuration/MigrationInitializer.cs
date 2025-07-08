namespace DiscountCodesGenerator.Configuration;

public static class MigrationInitializer
{
    public static void ApplyMigrations(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("Startup.Migration");
        var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();

        using var db = dbContextFactory.CreateDbContext();
        try
        {
            logger.LogInformation("⚙️ Running database migrations...");
            db.Database.Migrate();
            logger.LogInformation("✔️ Database migration complete.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ An error occurred while running migrations.");
            throw;
        }
    }
}
