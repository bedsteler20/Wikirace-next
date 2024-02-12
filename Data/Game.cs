#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Wikirace.Data;


public class Game {
    [Key] public string Id { get; set; }

    [Required]
    public IEnumerable<Player> Players { get; set; }

    public string StartPage { get; set; }
    public string EndPage { get; set; }
    public GameState State { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime FinishedAt { get; set; }
    public int MaxPlayers { get; set; }
    public string JoinCode { get; set; }
    public GameType GameType { get; set; }
}
