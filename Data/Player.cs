#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Wikirace.Data;

public class Player {
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

    public string HashEmail() {
        if (User == null) return string.Empty;

        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(User.Email));
        return Convert.ToBase64String(bytes);
    }

}
