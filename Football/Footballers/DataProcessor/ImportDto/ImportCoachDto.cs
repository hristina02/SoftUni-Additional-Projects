using Footballers.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Coach")]
    public class ImportCoachDto
    {
        [Required]
        [MinLength(2)]
        [MaxLength(40)]

        public string Name { get; set; }

        public string Nationality { get; set; }

        public ImportFootballerDto[] Footballers { get; set; }
    }
}
