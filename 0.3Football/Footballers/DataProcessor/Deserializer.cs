namespace Footballers.DataProcessor
{
    using Footballers.Data;
    using System.ComponentModel.DataAnnotations;
    using Footballers.Extentions;
    using System.Diagnostics;
    using System.Security.AccessControl;
    using System.Text;
    using Footballers.DataProcessor.ImportDto;
    using Footballers.Data.Models;
    using System.Globalization;
    using Footballers.Data.Models.Enums;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            ImportCoachDto[] coachDtos = xmlString.DeserializeFromXml<ImportCoachDto[]>("Coaches");

            HashSet<Coach> coaches = new HashSet<Coach>();
            foreach (var coachDto in coachDtos)
            {

                if (!IsValid(coachDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }


                Coach coach = new()
                {
                    Name = coachDto.Name,
                    Nationality = coachDto.Nationality
                };

                foreach (var footballDto in coachDto.Footballers)
                {
                    if (!IsValid(footballDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    // string pattern = "dd/MM/yyyy";
                    DateTime csd;
                    if (!DateTime.TryParseExact(footballDto.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                                               DateTimeStyles.None,
                                               out csd))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime ced;
                    if (!DateTime.TryParseExact(footballDto.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                                               DateTimeStyles.None,
                                               out ced))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;

                    }

                    if (csd > ced)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    coach.Footballers.Add(new Footballer()
                    {
                        Name = footballDto.Name,
                        ContractStartDate = csd,
                        ContractEndDate = csd,
                        BestSkillType = (BestSkillType)footballDto.BestSkillType,
                        PositionType = (PositionType)footballDto.PositionType

                    });
                }

                coaches.Add(coach);
                sb.AppendLine(String.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count));
            }

            context.Coaches.AddRange(coaches);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        
        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            throw new NotImplementedException();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
