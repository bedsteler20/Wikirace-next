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
        modelBuilder.Entity<Game>()
            .HasMany(g => g.Players)
            .WithOne(p => p.Game)
            .HasForeignKey(p => p.GameId);
        modelBuilder.Entity<AppUser>()
            .HasMany(u => u.Players)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId);

        modelBuilder.Entity<Player>()
            .HasIndex(p => p.IsOwner)
            .IsUnique()
            .HasFilter("[IsOwner] = 1");

        modelBuilder.Entity<Player>()
            .HasIndex(p => p.IsWinner)
            .HasFilter("[IsWinner] = 1")
            .HasFilter("[IsWinner] = 0")
            .IsUnique();
    }
}
