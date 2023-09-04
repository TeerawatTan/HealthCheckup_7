using System;
using System.ComponentModel.DataAnnotations;

namespace HelpCheck_API.Models
{
    public class PsychiatristCheck
    {
        [Key]
        public int ID { get; set; }
        public int MemberID { get; set; }
        public int DoctorID { get; set; }
        public string Detail { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
