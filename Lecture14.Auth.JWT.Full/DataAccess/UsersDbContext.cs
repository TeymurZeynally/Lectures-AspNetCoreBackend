using Lecture14.Auth.JWT.Full.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lecture14.Auth.JWT.Full.DataAccess
{
    public class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var user = modelBuilder.Entity<User>();
            user.HasKey(x => x.Id);
            user.HasIndex(x => x.Uid).IsUnique();
            user.HasIndex(x => x.Login).IsUnique();
            user.Property(x => x.Login).HasMaxLength(128).IsRequired();
            user.Property(x => x.PasswordHash).HasMaxLength(256).IsRequired();
            user.Property(x => x.PasswordSalt).HasMaxLength(128).IsRequired();
            user.Property(x => x.Role).HasMaxLength(64).IsRequired();

            var refreshToken = modelBuilder.Entity<RefreshToken>();
            refreshToken.HasKey(x => x.Id);
            refreshToken.HasIndex(x => x.TokenHash).IsUnique();
            refreshToken.Property(x => x.TokenHash).HasMaxLength(128).IsRequired();
            refreshToken.HasOne(x => x.User).WithMany(x => x.RefreshTokens).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
