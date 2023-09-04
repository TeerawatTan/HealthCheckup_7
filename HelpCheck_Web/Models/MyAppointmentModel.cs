using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_Web.Models
{
    public class MyAppointmentModel
    {
        public int ID { get; set; }
        public int? DoctorID { get; set; }
        public string DoctorFullName { get; set; }
        public DateTimeOffset AppointmentDate { get; set; }
        public DateTimeOffset? AppointmentDateTimeStart { get; set; }
        public DateTimeOffset? AppointmentDateTimeEnd { get; set; }
        public int MemberID { get; set; }
        public int QueueNo { get; set; }
        public int AppointmentID { get; set; }
    }
}
