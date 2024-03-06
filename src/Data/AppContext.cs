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

        modelBuilder.Entity<AppUser>().HasMany(u => u.Players).WithOne(p => p.User).HasForeignKey(p => p.UserId);
        modelBuilder.Entity<Player>().HasOne(p => p.User).WithMany(u => u.Players).HasForeignKey(p => p.UserId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        var dbType = Environment.GetEnvironmentVariable("DB_TYPE") ?? "sqlite";
        if (dbType == "sqlite") {
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? "Data Source=wikirace.db";
            optionsBuilder.UseSqlite(connectionString);
        } else if (dbType == "postgres") {
            throw new Exception("Postgres not implemented");
        } else {
            throw new Exception("Unknown DB_TYPE" + dbType);
        }
    }
}
