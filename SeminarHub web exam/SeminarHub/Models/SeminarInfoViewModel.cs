using SeminarHub.Data;
using System.Xml.Linq;

namespace SeminarHub.Models
{
    public class SeminarInfoViewModel
    {
        public SeminarInfoViewModel(
            int id,
            string topic,
            string lecturer,
            string category,
            DateTime dateAndTime,
            string organizer)
        {
           
            this.Id = id;
            this.Topic = topic;
            this.Lecturer = lecturer;
            this.Category = category;
            this.DateAndTime=dateAndTime.ToString(DataValidationConstants.DateFormat);
            this.Organizer = organizer;

        }


        public int Id { get; set; }

        public string Topic { get; set; } 

        public string Lecturer { get; set; }

        public string Category { get; set; } 

        public string DateAndTime{ get; set; }

        public string Organizer {  get; set; }
    }
}
