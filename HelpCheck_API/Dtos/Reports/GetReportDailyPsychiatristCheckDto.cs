using HelpCheck_API.Constants;
using System;

namespace HelpCheck_API.Dtos.Reports
{
    public class GetReportDailyPsychiatristCheckDto
    {

        public int MemberID { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Hn { get; set; }
        public string IdCard { get; set; }
        public string WorkPlaceName { get; set; }
        public string JobTypeName { get; set; }
        internal DateTime? BirthDate { get; set; }
        public int? Age { get { return BirthDate.HasValue ? Calculate.CalculateAge(BirthDate.Value) : null; } }
        internal int? Sex { get; set; }
        public int? Gender { get { return Sex.HasValue ? Sex.Value == 1 ? 2 : 1 : null; } }
        public string TreatmentName { get; set; }
        public bool? Question2QOne { get; set; }
        public bool? Question2QTwo { get; set; }
        public int Question9QOne { get; set; }
        public int Question9QTwo { get; set; }
        public int Question9QThree { get; set; }
        public int Question9QFour { get; set; }
        public int Question9QFive { get; set; }
        public int Question9QSix { get; set; }
        public int Question9QSeven { get; set; }
        public int Question9QEight { get; set; }
        public int Question9QNine { get; set; }
        public int SumQuestion9Q { get { return Question9QOne + Question9QTwo + Question9QThree + Question9QFour + Question9QFive + Question9QSix + Question9QSeven + Question9QEight + Question9QNine; } }
    }
}
