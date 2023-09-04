using System;

namespace HelpCheck_API.Dtos.Appointments
{
    public class AddBookingAppointmentDto
    {
        internal string AccessToken { get; set; }
        public int? DoctorID { get; set; }
        public int AppointmentID { get; set; }
        internal bool IsBooked { get; set; }
    }

    public class AddAppointmentSettingDto
    {
        internal string AccessToken { get; set; }
        public int? DoctorID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime? AppointmentTimeStart { get; set; }
        public DateTime? AppointmentTimeEnd { get; set; }
        public int? MaximumPatient { get; set; }
    }
}