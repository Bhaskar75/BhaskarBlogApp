using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BhaskarBlogApp.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Seed Roles (User,Admin,SuperAdmin)

            var adminRoleId = "f2bdc830-7f7f-4f5b-a1e3-0d5e8326a111";
            var superAdminRoleId = "5abfa2a9-31c2-489b-84e9-e52a0ad35d22";
            var userRoleId = "c357b63a-8c44-4994-b6b6-c0a33cb7581e";
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name="Admin",
                    NormalizedName="Admin",
                    Id=adminRoleId,
                    ConcurrencyStamp=adminRoleId
                },
                new IdentityRole
                {
                    Name="SuperAdmin",
                    NormalizedName="SuperAdmin",
                    Id=superAdminRoleId,
                    ConcurrencyStamp=superAdminRoleId
                },
                new IdentityRole
                {
                    Name="User",
                    NormalizedName="User",
                    Id=userRoleId,
                    ConcurrencyStamp=userRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
            //Seed SuperAdminUser

            var superAdminId = "c701b769-7816-4c3c-b09f-2499c77f1bb5";

            var superAdminUser = new IdentityUser
            {
                UserName="superadmin@bloggie.com",
                Email="superadmin@bloggie.com",
                NormalizedEmail="superadmin@bloggie.com",
                NormalizedUserName="superadmin@bloggie.com",
                Id=superAdminId
            };

            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdminUser, "Superadmin@123");
            builder.Entity<IdentityUser>().HasData(superAdminUser);

            //Add All roles to SuperAdminUser

            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId=adminRoleId,
                    UserId=superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId=superAdminRoleId,
                    UserId=superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId=userRoleId,
                    UserId=superAdminId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
