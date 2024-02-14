namespace Wikirace.Data;

/// <summary>
/// Represents the state of the game.
/// </summary>
public enum GameState {
    WaitingForPlayers,
    InProgress,
    Finished,
    /// <summary>
    /// The game was ended by the owner. This is different from Finished.
    /// </summary>
    Ended 
}
