using SeminarHub.Data;

namespace SeminarHub.Models
{
    public class SeminarDetailsViewModel
    {
        public SeminarDetailsViewModel(
           int id,
           string topic,
           string lecturer,
           string category,
           string details,
           DateTime dateAndTime,
           string organizer,
           int duration)
        {

            this.Id = id;
            this.Topic = topic;
            this.Lecturer = lecturer;
            this.Category = category;
            this.Details = details;
            this.DateAndTime = dateAndTime.ToString(DataValidationConstants.DateFormat);
            this.Organizer = organizer;
            this.Duration = duration;

        }


        public int Id { get; set; }

        public string Topic { get; set; }

        public string Lecturer { get; set; }

        public int Duration { get; set; }

        public string Category { get; set; }

        public string Details { get; set; }

        public string DateAndTime { get; set; }

        public string Organizer { get; set; }
    }
}
