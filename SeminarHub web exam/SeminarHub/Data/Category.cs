using System.ComponentModel.DataAnnotations;
using static SeminarHub.Data.DataValidationConstants;

namespace SeminarHub.Data
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(CategoryNameMaximumLength)]
        public string Name { get; set; } = string.Empty;

        public IEnumerable<Seminar> Seminars { get; set; }= new List<Seminar>();
    }
}
