using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SeminarHub.Data.DataValidationConstants;

namespace SeminarHub.Data
{
    public class Seminar
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(SeminarTopicMaximumLength)]
        public string Topic { get; set; }= string.Empty;

        [Required]
        [MaxLength(SeminarLecturerMaximumLength)]
        public string Lecturer { get; set; }= string.Empty;

        [Required]
        [MaxLength(SeminarDetailsMaximumLength)]
        public string Details { get; set; }=string.Empty;

        [Required]
        public string OrganizerId { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(OrganizerId))]
        public IdentityUser Organizer { get; set; } = null!;

        [Required]
        public DateTime DateAndTime { get; set; }

        [Range(DurationMinimum,DurationMaximum)]
        public int Duration { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        public IList<SeminarParticipant> SeminarsParticipants { get; set; }=new List<SeminarParticipant>();
    }
}
