using System;
using System.ComponentModel.DataAnnotations;

namespace HelpCheck_API.Models
{
    public class AmedAnswerDetail
    {
        [Key]
        public int AnswerDetaiID { get; set; }
        public int MemberID { get; set; }
        public int QuestionID { get; set; }
        public int ChoiceID { get; set; }
        public string QuestionNum { get; set; }
        public string ChoiceNum { get; set; }
        public string AnswerKeyIn { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}