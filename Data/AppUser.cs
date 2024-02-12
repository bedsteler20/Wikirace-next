#nullable disable
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Wikirace.Data;

public class AppUser : IdentityUser {
    public ICollection<Player> Players { get; set; }
    public bool IsAnonymous { get; set; }

    [NotMapped]
    public string DisplayName => IsAnonymous ? "Anonymous" : UserName;
}


