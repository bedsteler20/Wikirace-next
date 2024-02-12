using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Wikirace.Data;


public partial class AppDbContext : IdentityDbContext<AppUser> {
    public DbSet<Game> Games { get; set; }
    public DbSet<Player> Players { get; set; }

    public AppDbContext() {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Game>().HasMany(g => g.Players).WithOne(p => p.Game).OnDelete(DeleteBehavior.Cascade);
    }
}
