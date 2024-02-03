using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HouseRentingSystem.Data.Models;

namespace HouseRentingSystem.Data.Configurations
{
    public class HouseEntityConfigurations:IEntityTypeConfiguration<House>
    {
        public void Configure(EntityTypeBuilder<House> builder)
        {
            builder
                .HasOne(h=>h.Category)
                .WithMany(c=>c.Houses)
                .HasForeignKey(h=>h.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
              .HasOne(h => h.Agent)
              .WithMany(c => c.OwnedHouses)
              .HasForeignKey(h => h.AgentId)
              .OnDelete(DeleteBehavior.Restrict);


        }

    }
}
