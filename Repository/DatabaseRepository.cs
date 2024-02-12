﻿using Microsoft.EntityFrameworkCore;
using Wikirace.Data;

namespace Wikirace.Repository;
// TODO: Add logging
internal class DatabaseRepository : IRepository {
    private readonly AppDbContext _database;

    public DatabaseRepository(AppDbContext database) {
        _database = database;
    }

    /// <summary>
    /// Creates a new game with the specified parameters and saves it to the database.
    /// </summary>
    /// <param name="startPage">The starting page of the game.</param>
    /// <param name="endPage">The ending page of the game.</param>
    /// <param name="maxPlayers">The maximum number of players allowed in the game.</param>
    /// <param name="gameType">The type of the game.</param>
    /// <returns>The newly created game.</returns>
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
            Id = Guid.NewGuid().ToString(),
        };
        _database.Games.Add(game);
        await _database.SaveChangesAsync();
        return game;
    }

    /// <summary>
    /// Ends the game with the specified game ID.
    /// </summary>
    /// <param name="gameId">The ID of the game to end.</param>
    /// <returns>A task representing the asynchronous operation. The task result is the ended game, or null if the game was not found.</returns>
    public Task<Game?> EndGame(string gameId) {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Retrieves a game from the database based on the provided game ID.
    /// </summary>
    /// <param name="gameId">The ID of the game to retrieve.</param>
    /// <returns>The retrieved game, or null if no game with the specified ID is found.</returns>
    public Task<Game?> GetGame(string gameId) {
        return _database.Games.FirstOrDefaultAsync(g => g.Id == gameId);
    }

    /// <summary>
    /// Retrieves a game from the database based on the provided join code.
    /// </summary>
    /// <param name="joinCode">The join code of the game to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the retrieved game, or null if no game is found.</returns>
    public Task<Game?> GetGameByJoinCode(string joinCode) {
        return _database.Games.FirstOrDefaultAsync(g => g.JoinCode == joinCode);
    }

    /// <summary>
    /// Joins a game with the specified game ID, player name, user ID, and owner status.
    /// </summary>
    /// <param name="gameId">The ID of the game to join.</param>
    /// <param name="playerName">The name of the player joining the game.</param>
    /// <param name="userId">The ID of the user joining the game.</param>
    /// <param name="isOwner">Specifies whether the player joining the game is the owner.</param>
    /// <returns>The joined game if successful, or null if the game does not exist, is not in the waiting state, or has reached the maximum number of players.</returns>
    public async Task<Game?> JoinGame(string gameId, string playerName, string userId, bool isOwner = false) {
        var game = _database.Games.Find(gameId);
        if (game == null) return null;

        if (game.State != GameState.WaitingForPlayers) return null;

        if ((game.Players?.Count() ?? 0) >= game.MaxPlayers) return null;

        var player = new Player {
            Username = playerName,
            UserId = userId,
            IsOwner = isOwner,
            GameId = gameId,
            JoinedAt = DateTime.Now,
            LastActiveAt = DateTime.Now,
            CurrentPage = game.StartPage,
            Id = Guid.NewGuid().ToString(),
        };

        _database.Players.Add(player);
        await _database.SaveChangesAsync();
        return game;
    }

    /// <summary>
    /// Starts a game with the specified game ID.
    /// </summary>
    /// <param name="gameId">The ID of the game to start.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the started game, or null if the game was not found.</returns>
    public Task<Game?> StartGame(string gameId) {
        throw new NotImplementedException();
    }
}


/// <summary>
/// Provides extension methods for configuring the database repository in the service collection.
/// </summary>
public static class DatabaseRepositoryExtensions {
    /// <summary>
    /// Adds the database repository implementation to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the repository to.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddDatabaseRepository(this IServiceCollection services) {
        services.AddScoped<IRepository, DatabaseRepository>();
        return services;
    }
}