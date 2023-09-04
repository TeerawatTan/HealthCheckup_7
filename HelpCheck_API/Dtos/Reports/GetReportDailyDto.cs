using HelpCheck_API.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Dtos.Reports
{
    public class GetReportDailyDto
    {
        internal int ID { get; set; }
        public DateTime Date { get; set; }
        public int RegisterAmount { get; set; }
        public int Amount { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdCard { get; set; }
        public string WorkPlaceName { get; set; }
        public string JobTypeName { get; set; }
        internal DateTime? BirthDate { get; set; }
        public int? Age { get { return BirthDate != null ? Calculate.CalculateAge(BirthDate.Value) : null; } }
        internal int? Sex { get; set; }
        public int? Gender { get { return Sex.HasValue ? Sex.Value == 1 ? 2 : 1 : null; } }
        public bool? IsHealthCheck { get; set; }
        public bool? IsScreening { get; set; }
        public bool? IsWeight { get; set; }
        public bool? IsHeight { get; set; }
        public bool? IsBmi { get; set; }
        public bool? IsWaistline { get; set; }
        public bool? IsBloodPressure { get; set; }
        public bool? IsDentalCheck { get; set; }
        public bool? IsDoctorCheck { get; set; }
        public bool? IsBloodCheck { get; set; }
        public bool? IsPapSmear { get; set; }
        public bool? IsXray { get; set; }
    }

    public class GetReportDailyDoctorCheckDto
    {
        public DateTime Date { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        internal DateTime? BirthDate { get; set; }
        public int? Age { get { return BirthDate.HasValue ? Calculate.CalculateAge(BirthDate.Value) : null; } }
        public string Province { get; set; }
        public bool? IsBcc { get; set; }
        public bool? IsPopulation { get; set; }
        public bool? IsOfficer { get; set; }
        public bool? IsSoldier { get; set; }
    }
}
