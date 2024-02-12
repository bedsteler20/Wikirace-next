using Microsoft.AspNetCore.Identity;

namespace Wikirace.Data;

public class AppUser : IdentityUser {
    public ICollection<Player> Players { get; set; }
}


