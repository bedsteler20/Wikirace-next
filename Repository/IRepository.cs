using Wikirace.Data;

namespace Wikirace.Repository;

public interface IRepository {
    public Task<Game?> GetGame(string gameId);
    public Task<Game?> GetGameByJoinCode(string joinCode);
    public Task<Game> CreateGame(string startPage, string endPage, int maxPlayers, GameType gameType);
    public Task<Game?> JoinGame(string gameId, string playerName, string userId, bool isOwner = false);
    public Task<Game?> StartGame(string gameId);
    public Task<Game?> EndGame(string gameId);

}
