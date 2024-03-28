#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Wikirace.Data;

public class AppUser : IdentityUser {
    [Required]
    public IEnumerable<Player> Players { get; set; }
    public bool IsAnonymous { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public int Wins { get; set; }

    [NotMapped]
    public string DisplayName => IsAnonymous ? "Anonymous" : UserName;

    [NotMapped]
    public string AvatarUrl => GravatarSharp.GravatarController.GetImageUrl(Email ?? "example@example.com", 100);
}


