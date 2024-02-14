#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wikirace.Data;

public class Player
{
    [Key]
    [Required]
    public string Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    public AppUser User { get; set; }

    [Required]
    public string GameId { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string CurrentPage { get; set; }

    [Required]
    public int Clicks { get; set; }

    [Required]
    public DateTime JoinedAt { get; set; }

    [Required]
    public DateTime LastActiveAt { get; set; }

    [Required]
    public bool IsOwner { get; set; }

    [Required]
    public bool IsWinner { get; set; }

    [ForeignKey("GameId")]
    public Game Game { get; set; }

}
