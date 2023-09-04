using System;
using System.ComponentModel.DataAnnotations;

namespace HelpCheck_API.Models
{
    public class ViewReportPsychiatristCheck
    {
        [Key]
        public int QuestionID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Hn { get; set; }
        public string IDCard { get; set; }
        public string WorkPlaceName { get; set; }
        public string JobTypeName { get; set; }
        internal DateTime? BirthDate { get; set; }
        internal int? Gender { get; set; }
        public string TreatmentName { get; set; }
        public bool? Q41 { get; set; }
        public bool? Q42 { get; set; }
        public int Q43 { get; set; }
        public int Q44 { get; set; }
        public int Q45 { get; set; }
        public int Q46 { get; set; }
        public int Q47 { get; set; }
        public int Q48 { get; set; }
        public int Q49 { get; set; }
        public int Q50 { get; set; }
        public int Q51 { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
