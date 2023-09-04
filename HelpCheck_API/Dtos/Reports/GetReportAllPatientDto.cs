namespace HelpCheck_API.Dtos.Reports
{
    public class GetReportAllPatientDto
    {
        public int SmokingBehavior { get; set; } = 0;
        public int AlcoholBehavior { get; set; } = 0;
        public int ExerciseBehavior { get; set; } = 0;
    }
}
