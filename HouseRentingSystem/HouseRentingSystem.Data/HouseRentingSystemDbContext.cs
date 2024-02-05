using HouseRentingSystem.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

namespace HouseRentingSystem.Data
{
    public class HouseRentingSystemDbContext : IdentityDbContext<ApplicationUser,IdentityRole<Guid>,Guid>
    {
        public HouseRentingSystemDbContext(DbContextOptions<HouseRentingSystemDbContext> options)
            : base(options)
        {


        }

        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<House> Houses { get; set; } = null!;

        public DbSet<Agent> Agents { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            Assembly configAssembly = Assembly.GetAssembly(typeof(HouseRentingSystemDbContext)) ??
                Assembly.GetExecutingAssembly();
            builder.ApplyConfigurationsFromAssembly(configAssembly);
            base.OnModelCreating(builder);
        }

    }
}
