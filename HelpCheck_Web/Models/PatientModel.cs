using System;
using System.Collections.Generic;

namespace HelpCheck_Web.Models
{
    public class PatientInfoModel
    {
        public int MemberID { get; set; }
        public int PhysicalSetID { get; set; }
        public List<PhysicalModel> PhysicalDetails { get; set; }
    }

    public class GetPatientAppointmentDto
    {
        public int ID { get; set; }
        public int? TitleID { get; set; }
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
    }

    public class GetPatientInfo
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public int? TitleID { get; set; }
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
    }
}
