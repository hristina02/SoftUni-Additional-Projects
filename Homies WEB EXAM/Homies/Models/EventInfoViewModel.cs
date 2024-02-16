using Homies.Data;

namespace Homies.Models
{
    public class EventInfoViewModel
    {

        public EventInfoViewModel(
           int id,
           string name,
           DateTime start,
           string type,
           string organizer)
        {
            Id = id;
            Name = name;
            Type = type;
            Organizer = organizer;
            Start = start.ToString(DataConstants.DateFormat);
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Start { get; set; }

        public string Type { get; set;}

        public string Organizer {  get; set; }
    }
}
