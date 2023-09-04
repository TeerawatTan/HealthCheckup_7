using System;

namespace HelpCheck_API.Dtos.Appointments
{
    public class GetAppointmentDto
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public int? AppointmentID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime? AppointmentDateTimeStart { get; set; }
        public DateTime? AppointmentDateTimeEnd { get; set; }
        public int QueueNo { get; set; }
        public int? DoctorID { get; set; }
        public string DoctorFullName { get; set; }
    }

    public class GetAppointmentSettingDto
    {
        public int ID { get; set; }
        public int? DoctorID { get; set; }
        public string DoctorFullName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime? AppointmentDateTimeStart { get; set; }
        public DateTime? AppointmentDateTimeEnd { get; set; }
        public int Booked { get; set; }
        public int MaximunBooked { get; set; }
    }

    public class GetCountAppointmentDto
    {
        public int ID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int Booked { get; set; }
        public int MaximumBooking { get; set; }
    }

    public class FilterAppointmentDto
    {
        public DateTime? AppointmnentDate { get; set; }
    }

    public class GetAppointmentDetailDto
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public string MemberIdCard { get; set; }
        public string MemberHn { get; set; }
        public int AppointmentID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime AppointmentDateTimeStart { get; set; }
        public DateTime AppointmentDateTimeEnd { get; set; }
        public int? TreatmentID { get; set; }
        public string TreatmentName { get; set; }
        public string Agency { get; set; }
    }
}