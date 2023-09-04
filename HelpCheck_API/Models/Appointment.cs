using System;
using System.ComponentModel.DataAnnotations;

namespace HelpCheck_API.Models
{
    public class Appointment
    {
        [Key]
        public int ID { get; set; }
        public int MemberID { get; set; }
        public int? AppointmentID { get; set; }
        public int? DoctorID { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int QueueNo { get; set; }
        public bool IsBooked { get; set; }
        public bool IsReserve { get; set; }
    }

    public class AppointmentSetting
    {
        [Key]
        public int ID { get; set; }
        public int? DoctorID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime? AppointmentDateTimeStart { get; set; }
        public DateTime? AppointmentDateTimeEnd { get; set; }
        public int MaximumPatient { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}