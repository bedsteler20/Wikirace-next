using Wikirace.Data;

namespace Wikirace.Repository;

/// <summary>
/// Represents a repository for managing game data.
/// </summary>
public interface IRepository {
    /// <summary>
    /// Retrieves a game by its unique identifier.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game.</param>
    /// <returns>The game object if found, otherwise null.</returns>
    public Task<Game?> GetGame(string gameId);

    /// <summary>
    /// Retrieves a game by its join code.
    /// </summary>
    /// <param name="joinCode">The join code of the game.</param>
    /// <returns>The game object if found, otherwise null.</returns>
    public Task<Game?> GetGameByJoinCode(string joinCode);

    /// <summary>
    /// Creates a new game with the specified parameters.
    /// </summary>
    /// <param name="startPage">The starting page of the game.</param>
    /// <param name="endPage">The ending page of the game.</param>
    /// <param name="maxPlayers">The maximum number of players allowed in the game.</param>
    /// <param name="gameType">The type of the game.</param>
    /// <returns>The created game object.</returns>
    public Task<Game> CreateGame(string startPage, string endPage, int maxPlayers, GameType gameType);

    /// <summary>
    /// Joins a player to an existing game.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game.</param>
    /// <param name="playerName">The name of the player.</param>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="isOwner">Indicates whether the player is the owner of the game.</param>
    /// <returns>The updated game object if successful, otherwise null.</returns>
    public Task<Game?> JoinGame(string gameId, string playerName, string userId, bool isOwner = false);

    /// <summary>
    /// Starts a game with the specified identifier.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game.</param>
    /// <returns>The updated game object if successful, otherwise null.</returns>
    public Task<Game?> StartGame(string gameId);

    /// <summary>
    /// Ends a game with the specified identifier.
    /// </summary>
    /// <param name="gameId">The unique identifier of the game.</param>
    /// <returns>The updated game object if successful, otherwise null.</returns>
    public Task<Game?> EndGame(string gameId);

    public Task UpdatePage(string gameId, string userId, string page);
    /// <summary>
    /// Kicks a player from a game.
    /// </summary>
    /// <param name="gameId">The ID of the game.</param>
    /// <param name="playerId">The ID of the player to kick.</param>
    /// <returns>A task representing the asynchronous operation. The task result is the updated game object, or null if the game or player does not exist.</returns>
    public Task<Game?> KickPlayer(string gameId, string playerId);
}
