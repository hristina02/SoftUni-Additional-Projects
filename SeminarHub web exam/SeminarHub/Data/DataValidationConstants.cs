using Microsoft.EntityFrameworkCore.Storage;

namespace SeminarHub.Data
{
    public static class DataValidationConstants
    {
        public const int SeminarTopicMinimumLength = 3;
        public const int SeminarTopicMaximumLength = 100;

        public const int SeminarLecturerMinimumLength = 5;
        public const int SeminarLecturerMaximumLength = 60;

        public const int SeminarDetailsMinimumLength = 10;
        public const int SeminarDetailsMaximumLength = 500;

        public const string DateFormat = "dd/MM/yyyy HH:mm";

        public const int DurationMinimum = 30;
        public const int DurationMaximum = 180;

        public const int CategoryNameMinimumLength = 3;
        public const int CategoryNameMaximumLength = 50;
    }
}
