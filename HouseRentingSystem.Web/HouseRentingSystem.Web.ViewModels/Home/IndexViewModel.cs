namespace HouseRentingSystem.Web.ViewModels.Home
{
    
    using static HouseRentingSystem.Common.EntityValidationConstants;

    public class IndexViewModel 
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;
    }
}