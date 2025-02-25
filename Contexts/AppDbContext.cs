using dotnet6_webapi.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet6_webapi.Contexts;

public class AppDbContext : DbContext
{
    public DbSet<User>? Users { get; set; }
    public DbSet<RefreshToken>? RefreshTokens { get; set; }
    public DbSet<UserRefreshToken>? UserRefreshTokens { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 設定 User 和 RefreshToken 的多對多關聯
        modelBuilder.Entity<UserRefreshToken>()
            .HasKey(urt => new { urt.UserId, urt.RefreshTokenId });

        modelBuilder.Entity<UserRefreshToken>()
            .HasOne(urt => urt.User)
            .WithMany(u => u.UserRefreshTokens)
            .HasForeignKey(urt => urt.UserId);

        modelBuilder.Entity<UserRefreshToken>()
            .HasOne(urt => urt.RefreshToken)
            .WithMany(rt => rt.UserRefreshTokens)
            .HasForeignKey(urt => urt.RefreshTokenId);
    }
}
