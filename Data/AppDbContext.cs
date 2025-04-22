using GaavLearnAPIs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GaavLearnAPIs.Data
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
        }
         protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Change default Identity table name
        builder.Entity<AppUser>().ToTable("T_users");
        // Optional: Rename other Identity tables if needed
        builder.Entity<IdentityRole>().ToTable("T_roles");
        builder.Entity<IdentityUserRole<string>>().ToTable("T_user_roles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("T_user_claims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("T_user_logins");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("T_role_claims");
        builder.Entity<IdentityUserToken<string>>().ToTable("T_user_tokens");
    }

    }
}
