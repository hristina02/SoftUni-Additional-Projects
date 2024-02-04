﻿using Microsoft.EntityFrameworkCore;
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
                .Property(x => x.CreatedOn)
                .HasDefaultValue(DateTime.UtcNow);
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

            builder.HasData(this.GenerateHouses());

        }

        private House[] GenerateHouses()
        {
            ICollection<House> houses = new HashSet<House>();

            House house;

            house = new House()
            {
                Title = "Big House Marina",
                Address = "North London, UK (near the border)",
                Description = "A big house for your whole family. Don't miss to buy a house with three bedrooms.",
                ImageUrl = "https://www.luxury-architecture.net/wp-content/uploads/2017/12/1513217889-7597-FAIRWAYS-010.jpg",
                PricePerMonth = 2100.00M,
                CategoryId = 3,
                AgentId = Guid.Parse("48245cda-4a0f-4bc6-90ce-7f8c5493c91b"), //AgentId
                RenterId = Guid.Parse("f2382942-61f6-4ff3-844e-ab4ddb08e95e") //UserId
            };
            houses.Add(house);

            house = new House()
            {
                Title = "Family House Comfort",
                Address = "Near the Sea Garden in Burgas, Bulgaria",
                Description = "It has the best comfort you will ever ask for. With two bedrooms, it is great for your family.",
                ImageUrl = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/179489660.jpg?k=2029f6d9589b49c95dcc9503a265e292c2cdfcb5277487a0050397c3f8dd545a&o=&hp=1",
                PricePerMonth = 1200.00M,
                CategoryId = 2,
                AgentId = Guid.Parse("48245cda-4a0f-4bc6-90ce-7f8c5493c91b"), //AgentId
            };
            houses.Add(house);

            house = new House()
            {
                Title = "Grand House",
                Address = "Boyana Neighbourhood, Sofia, Bulgaria",
                Description = "This luxurious house is everything you will need. It is just excellent.",
                ImageUrl = "https://i.pinimg.com/originals/a6/f5/85/a6f5850a77633c56e4e4ac4f867e3c00.jpg",
                PricePerMonth = 2000.00M,
                CategoryId = 2,
                AgentId = Guid.Parse("48245cda-4a0f-4bc6-90ce-7f8c5493c91b"), //AgentId
            };
            houses.Add(house);

            return houses.ToArray();
        }

    }
}
