using System;
using System.ComponentModel.DataAnnotations;

namespace HelpCheck_API.Models
{
    public class AnswerPhysical
    {
        [Key]
        public int AnsphyID { get; set; }
        public int? MemberID { get; set; }
        public int? PhysicalChoiceID { get; set; }
        public string AnsphyKeyIn { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int PhysicalSetID { get; set; }
    }
}