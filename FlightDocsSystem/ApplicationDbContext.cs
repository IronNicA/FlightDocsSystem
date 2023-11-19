using FlightDocsSystem.Models.ManagementModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FlightDocsSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Doc> Docs { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Doc>()
                .Property(d => d.CreateDate)
                .HasColumnType("datetime");

            builder.Entity<Flight>()
                .Property(f => f.CreateDate)
                .HasColumnType("datetime");

            SeedRolesAndAdminUser(builder);
        }

        private void SeedRolesAndAdminUser(ModelBuilder builder)
        {
            var roles = new[]
            {
            new Role { Id = 1, Name = "Admin", NormalizedName = "ADMIN", Creator = "System", Permission = 2},
            new Role { Id = 2, Name = "Crew", NormalizedName = "CREW", Creator = "System" , Permission = 1},
            new Role { Id = 3, Name = "Pilot", NormalizedName = "PILOT", Creator = "System", Permission = 2},
        };

            builder.Entity<Role>().HasData(roles);

            var adminUser = new User
            {
                Id = 1,
                UserName = "admin",
                Email = "admin@example.com",
                // Password : System_Admin
                PasswordHash = Convert.FromBase64String("p/rPEX/Hk1O35+2IsaoO2Cg2ugArhQKLbs1zBE6R/KBtyEmcBfUiVfvs3+tGnqcFkIVkcIx+zFvg6N4RAKOUWA=="),
                PasswordSalt = Convert.FromBase64String("iIb0h933oDXEIGlWypyO7xNrnM50C6lWIkK681C+EIDMOgQval1R16QqwRgkvJqT8CHMMFjrKI7yV3QIQ/EXCHRaEcp4njmzMKbmfWc+FJWu7DLj3mRdceVRXVqzR8QEzhWrNunvuEdoPFyoMfcoSpGiH8eYrTypWLh41k5EeZY="),
                Phone = "1234567890",
                Role = "Admin"
            };

            builder.Entity<User>().HasData(adminUser);
        }
    }
}
