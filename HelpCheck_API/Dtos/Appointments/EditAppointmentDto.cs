using System;

namespace HelpCheck_API.Dtos.Appointments
{
    public class EditAppointmentSettingDto
    {
        internal string AccessToken { get; set; }
        internal int ID { get; set; }
        public int? DoctorID { get; set; }
        public DateTime? AppointmentDateTimeStart { get; set; }
        public DateTime? AppointmentDateTimeEnd { get; set; }
        public int? MaximumPatient { get; set; }
    }

    public class UnReserveDto
    {
        public bool IsReserve { get; set; }
    }
}