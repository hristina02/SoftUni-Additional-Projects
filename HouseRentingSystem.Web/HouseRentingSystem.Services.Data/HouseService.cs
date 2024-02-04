using HouseRentingSystem.Services.Data.Interfaces;
using HouseRentingSystem.Web.Data;
using HouseRentingSystem.Web.ViewModels.Home;
using Microsoft.EntityFrameworkCore;


namespace HouseRentingSystem.Services.Data
{
    public class HouseService : IHouseService
    {
        private readonly HouseRentingDbContext dbContext;

        public HouseService(HouseRentingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<IndexViewModel>> LastThreeHouseAsync()
        {
            IEnumerable<IndexViewModel> lastThreeHouses = await this.dbContext
                .Houses
                .OrderByDescending(h => h.CreatedOn)
                .Take(3)
                .Select(h => new IndexViewModel()
                {
                    Id = h.Id.ToString(),
                    Title = h.Title,
                    ImageUrl = h.ImageUrl

                })
                .ToArrayAsync();
            return lastThreeHouses;
        }
    } 
}
