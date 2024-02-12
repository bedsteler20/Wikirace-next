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
        modelBuilder.Entity<Game>().HasMany(g => g.Players).WithOne(p => p.Game).HasForeignKey(p => p.GameId);
        modelBuilder.Entity<Player>().HasOne(p => p.Game).WithMany(g => g.Players).HasForeignKey(p => p.GameId);
    }
}
