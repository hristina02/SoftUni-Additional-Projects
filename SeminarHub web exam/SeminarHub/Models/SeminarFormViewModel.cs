
using System.ComponentModel.DataAnnotations;
using static SeminarHub.Data.DataValidationConstants;

namespace SeminarHub.Models
{
    public class SeminarFormViewModel
    {
        [Required]
        [StringLength(SeminarTopicMaximumLength, MinimumLength =SeminarTopicMinimumLength)]
        public string Topic{ get; set; } = string.Empty;

        [Required]
        [StringLength(SeminarLecturerMaximumLength,MinimumLength=SeminarLecturerMinimumLength)]
        public string Lecturer { get; set; } = string.Empty;

        [Required]
        [StringLength(SeminarDetailsMaximumLength,MinimumLength =SeminarDetailsMinimumLength)]

        public string Details { get; set; } = string.Empty;

        [Required]

        public string DateAndTime { get; set; }= string.Empty;

        [Range(DurationMinimum, DurationMaximum)]
        public int Duration { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public IEnumerable<CategoryViewModel>Categories { get; set; } = new List<CategoryViewModel>();

    }
}
