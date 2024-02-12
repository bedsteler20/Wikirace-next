using Microsoft.EntityFrameworkCore;
using Wikirace.Data;

namespace Wikirace.Repository;

internal class DatabaseRepository : IRepository {
    private readonly AppDbContext _database;
    
    public DatabaseRepository(AppDbContext database) {
        _database = database;
    }

    public async Task<Game> CreateGame(string startPage, string endPage, int maxPlayers, GameType gameType) {
        var game = new Game {
            StartPage = startPage,
            EndPage = endPage,
            MaxPlayers = maxPlayers,
            GameType = gameType,
            JoinCode = Guid.NewGuid().ToString()[..6],
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            State = GameState.WaitingForPlayers,
        };
        _database.Games.Add(game);
        await _database.SaveChangesAsync();
        return game;
    }

    public Task<Game?> EndGame(string gameId) {
        throw new NotImplementedException();
    }

    public Task<Game?> GetGame(string gameId) {
        return _database.Games.FirstOrDefaultAsync(g => g.Id == gameId);
    }

    public Task<Game?> GetGameByJoinCode(string joinCode) {
        return _database.Games.FirstOrDefaultAsync(g => g.JoinCode == joinCode);
    }

    public async Task<Game?> JoinGame(string gameId, string playerName, string userId, bool isOwner = false) {
        var game = _database.Games.Find(gameId);
        if (game == null) return null;

        if (game.State != GameState.WaitingForPlayers) return null;

        if (game.Players.Count >= game.MaxPlayers) return null;

        var player = new Player {
            Username = playerName,
            UserId = userId,
            IsOwner = isOwner,
            GameId = gameId,
            JoinedAt = DateTime.Now,
            LastActiveAt = DateTime.Now,
            CurrentPage = game.StartPage,
        };

        _database.Players.Add(player);
        await _database.SaveChangesAsync();
        return game;
    }

    public Task<Game?> StartGame(string gameId) {
        throw new NotImplementedException();
    }
}


public static class DatabaseRepositoryExtensions {
    public static IServiceCollection AddDatabaseRepository(this IServiceCollection services) {
        services.AddScoped<IRepository, DatabaseRepository>();
        return services;
    }
}