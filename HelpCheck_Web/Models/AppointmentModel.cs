using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_Web.Models
{
    public class AppointmentModel
    {
        public int ID { get; set; }
        public int? DoctorID { get; set; }
        public string DoctorFullName { get; set; }
        public DateTimeOffset AppointmentDate { get; set; }
        public DateTimeOffset? AppointmentDateTimeStart { get; set; }
        public DateTimeOffset? AppointmentDateTimeEnd { get; set; }
        public int Booked { get; set; }
        public int MaximunBooked { get; set; }
        public string DateStr { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
    }

    public class CreateAppointmentDto
    {
        public int DoctorID { get; set; }
        public DateTimeOffset AppointmentDate { get; set; }
        public DateTimeOffset? AppointmentTimeStart { get; set; }
        public DateTimeOffset? AppointmentTimeEnd { get; set; }
        public int MaximumPatient { get; set; }
    }

    public class SubEditAppointmentDto
    {
        public int ID { get; set; }
        public int DoctorID { get; set; }
        public DateTimeOffset AppointmentDate { get; set; }
        public DateTimeOffset? AppointmentTimeStart { get; set; }
        public DateTimeOffset? AppointmentTimeEnd { get; set; }
        public int MaximumPatient { get; set; }
    }
    public class EditAppointmentDto
    {
        public int doctorID { get; set; }
        public DateTimeOffset? appointmentDateTimeStart { get; set; }
        public DateTimeOffset? appointmentDateTimeEnd { get; set; }
        public int maximumPatient { get; set; }
    }

    public class DownloadAwsAppointmentDto
    {
        public int Year { get; set; }
        
    }

    public class GetAnswerAllUserDto
    {
        public string MemberName { get; set; }
        public string MemberIdCard { get; set; }
        public string MemberHn { get; set; }
        public string QuestionName { get; set; }
        public string ChoiceName { get; set; }
        public string AsnwerKeyIn { get; set; }
    }

    public class DownLoadApp
    {
        public string Date { get; set; }
    }

    public class GetAllApp
    {
        public string MemberIdCard { get; set; }
        public string MemberHn { get; set; }
        public string MemberName { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string TreatmentName { get; set; }
        public string Agency { get; set; }


    }
    public class GetID
    {
        public string id { get; set; }
        public string hn { get; set; }
    }
}
