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
            logger.LogInformation("Running cleanup");
            var now = DateTimeOffset.UtcNow;
            var cutoff = now - options.AnonymousAccountLifetime;
            var cutoffGames = now - options.GameLifetime;


            foreach (var game in dbContext.Games.AsEnumerable()) {
                if (game.UpdatedAt < cutoffGames) {
                    logger.LogInformation($"Deleting game {game.Id}");
                    foreach (var player in game.Players) {
                        logger.LogInformation($"Deleting player {player.Id}");
                        dbContext.Players.Remove(player);
                    }
                    dbContext.Games.Remove(game);
                }
            }

            foreach (var user in dbContext.Users.AsEnumerable()) {
                if (user.IsAnonymous && user.CreatedAt < cutoff) {
                    logger.LogInformation($"Deleting user {user.Id}");
                    dbContext.Users.Remove(user);
                }
            }


            await dbContext.SaveChangesAsync();

        }, null, TimeSpan.Zero, options.CheckInterval);
    }

}

public static class DbCleanupServiceExtensions {
    public static IServiceCollection AddDbCleanupService(this IServiceCollection services) {

        var dbContext = services.BuildServiceProvider().GetService<AppDbContext>();
        var logger = services.BuildServiceProvider().GetService<ILogger<DbCleanupService>>();
        var options = new DbCleanupOptions();
        var service = new DbCleanupService(logger!, dbContext!, options);
        services.AddSingleton<DbCleanupService>(service);
        return services;
    }
}