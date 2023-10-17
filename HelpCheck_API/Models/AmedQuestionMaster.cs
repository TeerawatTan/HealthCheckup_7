using System;
using System.ComponentModel.DataAnnotations;

namespace HelpCheck_API.Models
{
    public class AmedQuestionMaster
    {
        [Key]
        public int QuestionID { get; set; }
        public string QuestionName { get; set; }
        public int? ChoiceID { get; set; }
        public DateTime? QuestionPeriod { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string QuestionNum { get; set; }
        public bool? IsActive { get; set; }
        public string QuestionGroup { get; set; }
    }

    public class AmedQuestionMapChoiceMaster
    {
        public int QuestionID { get; set; }
        public string QuestionNum { get; set; }
        public string QuestionName { get; set; }
        public int? ChoiceID { get; set; }
        public string ChoiceName { get; set; }
        public DateTime? QuestionPeriod { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int? Score { get; set; }
        public bool? IsActive { get; set; }
    }
}