using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WiseCrackCollector.Models;
using System.Reflection.Emit;

namespace WiseCrackCollector.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUser>(entity => entity.Property(m => m.Id).HasMaxLength(100));
            builder.Entity<IdentityRole>(entity => entity.Property(m => m.Id).HasMaxLength(100));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(100));
            builder.Entity<IdentityUserRole<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(100));
            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(100));
            builder.Entity<IdentityUserClaim<string>>(entity => entity.Property(m => m.Id).HasMaxLength(100));
            builder.Entity<IdentityUserClaim<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(100));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.Property(m => m.Id).HasMaxLength(100));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.Property(m => m.RoleId).HasMaxLength(100));

            /*builder.Entity<Group>()
                .HasMany(g => g.Wisecracks)
                .WithOne(wc => wc.Group)
                .HasForeignKey(wc => wc.GroupID)
                .OnDelete(DeleteBehavior.Restrict);*/

            builder.Entity<Group>()
             .HasMany(g => g.Wisecracks)
             .WithOne(w => w.Group)
             .OnDelete(DeleteBehavior.Restrict);
        }
        public DbSet<Wisecrack> Wisecracks { get; set; }
        public DbSet<Group> Groups { get; set; }
    }
}
