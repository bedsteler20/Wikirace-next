using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Wikirace.Data;

namespace Wikirace.Services;

public class DbCleanupOptions {
    public TimeSpan CheckInterval { get; set; } = TimeSpan.FromMinutes(5);
    public TimeSpan AnonymousAccountLifetime { get; set; } = TimeSpan.FromDays(7);
    public TimeSpan GameLifetime { get; set; } = TimeSpan.FromHours(4);
}

public class DbCleanupService : IHostedService {
    private readonly ILogger<DbCleanupService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly DbCleanupOptions _options;

    public DbCleanupService(ILogger<DbCleanupService> logger, IServiceScopeFactory scopeFactory, IOptions<DbCleanupOptions> options) {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _options = options.Value;
    }

    public Task StartAsync(CancellationToken cancellationToken) {
        _logger.LogInformation("DbCleanupService started");

        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var timer = new Timer(async _ => {
            _logger.LogInformation("Running cleanup");
            var now = DateTimeOffset.UtcNow;
            var cutoff = now - _options.AnonymousAccountLifetime;
            var cutoffGames = now - _options.GameLifetime;

            var accounts = await dbContext.Users
                .Where(a => a.IsAnonymous && a.CreatedAt < cutoff)
                .ToListAsync();

            var games = await dbContext.Games
                .Where(g => g.CreatedAt < cutoffGames)
                .ToListAsync();

            games.ForEach(g => dbContext.Players.RemoveRange(g.Players));
            dbContext.Users.RemoveRange(accounts);
            dbContext.Games.RemoveRange(games);

            await dbContext.SaveChangesAsync();

            _logger.LogInformation($"Removed {accounts.Count} accounts and {games.Count} games");
        }, null, TimeSpan.Zero, _options.CheckInterval);


        return Task.CompletedTask;

    }

    public Task StopAsync(CancellationToken cancellationToken) {
        throw new NotImplementedException();
    }
}

public static class DbCleanupServiceExtensions {
    public static IServiceCollection AddDbCleanupService(this IServiceCollection services, Action<DbCleanupOptions> configure) {
        services.Configure(configure);
        services.AddHostedService<DbCleanupService>();
        return services;
    }

    public static IServiceCollection AddDbCleanupService(this IServiceCollection services, IConfiguration configuration) {
        services.Configure<DbCleanupOptions>(configuration);
        services.AddHostedService<DbCleanupService>();
        return services;
    }

    public static IServiceCollection AddDbCleanupService(this IServiceCollection services) {
        services.AddHostedService<DbCleanupService>();
        return services;
    }

    public static IServiceCollection AddDbCleanupService(this IServiceCollection services, DbCleanupOptions options) {
        services.AddSingleton(options);
        services.AddHostedService<DbCleanupService>();
        return services;
    }
}