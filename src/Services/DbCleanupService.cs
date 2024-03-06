using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Wikirace.Data;

namespace Wikirace.Services;

public class DbCleanupOptions {
    public TimeSpan CheckInterval { get; set; } = TimeSpan.FromMinutes(5);
    public TimeSpan AnonymousAccountLifetime { get; set; } = TimeSpan.FromDays(7);
    public TimeSpan GameLifetime { get; set; } = TimeSpan.FromHours(4);
}

public class DbCleanupService {

    public DbCleanupService(ILogger<DbCleanupService> logger, AppDbContext dbContext, DbCleanupOptions options) {

        logger.LogInformation("DbCleanupService started");

        var timer = new Timer(async _ => {

            var oldAccounts = dbContext.Users
                                       .Where(u => u.IsAnonymous)
                                       .ToList()
                                       .Where(u => u.CreatedAt < DateTime.Now - options.AnonymousAccountLifetime);

            logger.LogInformation("Removing {} old accounts", oldAccounts.Count());

            dbContext.RemoveRange(oldAccounts);

            var oldGames = dbContext.Games
                                    .ToList()
                                    .Where(g => g.CreatedAt < DateTime.Now - options.GameLifetime);

            logger.LogInformation("Removing {} old games", oldGames.Count());
            dbContext.RemoveRange(oldGames);

            await dbContext.SaveChangesAsync();

        }, null, TimeSpan.Zero, options.CheckInterval);
    }
}


public static class DbCleanupServiceExtensions {
    public static IServiceCollection AddDbCleanupService(this IServiceCollection services) {
        var options = services.BuildServiceProvider().GetService<IOptions<DbCleanupOptions>>();
        var dbContext = services.BuildServiceProvider().GetService<AppDbContext>();
        var logger = services.BuildServiceProvider().GetService<ILogger<DbCleanupService>>();
        var service = new DbCleanupService(logger!, dbContext!, options?.Value ?? new DbCleanupOptions());
        services.AddSingleton(service);
        return services;
    }
}