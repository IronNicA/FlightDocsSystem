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

            // Seed roles
            SeedRoles(builder);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            var roles = new[]
            {
                new Role { Id = 1, Name = "Admin", NormalizedName = "ADMIN", Creator = "System", Permission = 2},
                new Role { Id = 2, Name = "Crew", NormalizedName = "CREW", Creator = "System" , Permission = 1},
                new Role { Id = 3, Name = "Pilot", NormalizedName = "PILOT", Creator = "System", Permission = 2},
            };

            builder.Entity<Role>().HasData(roles);
        }
    }
}
