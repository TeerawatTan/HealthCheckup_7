using HelpCheck_API.Constants;
using System;

namespace HelpCheck_API.Dtos.Patients
{
    public class GetPhysicalDto
    {
        public int AnsphyID { get; set; }
        public int MemberID { get; set; }
        public int? PhysicalChoiceID { get; set; }
        public string AnsphyKeyIn { get; set; }
    }

    public class FilterPatientAppointmentDto
    {
        public string search { get; set; }
    }

    public class FilterPatientDto
    {
        public string FullName { get; set; }
        public DateTime? AppointmentDate { get; set; }
    }

    public class GetPatientAppointmentDto
    {
        public int ID { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IDCard { get; set; }
        public string PhoneNo { get; set; }
        public string Agency { get; set; }
        public int? WorkPlaceID { get; set; }
        public string WorkPlaceName { get; set; }
        public int? JobTypeID { get; set; }
        public string JobTypeName { get; set; }
        public int? DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string Hn { get; set; }
    }

    public class GetAllPatientAppointmentDto
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IDCard { get; set; }
        public string PhoneNo { get; set; }
        public string Agency { get; set; }
        public int? WorkPlaceID { get; set; }
        public string WorkPlaceName { get; set; }
        public int? JobTypeID { get; set; }
        public string JobTypeName { get; set; }
        public int? AppointmentID { get; set; }
        public int? DoctorID { get; set; }
        public string DoctorName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Hn { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Age { get { return BirthDate != null ? Calculate.CalculateAge(BirthDate.Value) : null; } }
        public int? Sex { get; set; }
        public int? Gender { get { return Sex.HasValue ? Sex.Value == 1 ? 2 : 1 : null; } }
        public int? TreatmentID { get; set; }
        public string TreatmentName { get; set; }
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
        public int? RegisterAmount { get; set; }
        public int? Amount { get; set; }
    }

    public class GetPatientFromApiDto
    {
        public string id_card { get; set; }
        public string hn { get; set; }
        public string prename { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string birth_year { get; set; }
        public string birth_month { get; set; }
        public string birth_day { get; set; }
        public string sex { get; set; }
        public string depend { get; set; }
    }
}