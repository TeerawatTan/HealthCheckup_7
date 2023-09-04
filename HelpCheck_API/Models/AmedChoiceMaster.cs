using System;
using System.ComponentModel.DataAnnotations;

namespace HelpCheck_API.Models
{
    public class AmedChoiceMaster
    {
        [Key]
        public int ChoiceID { get; set; }
        public int? QuestionID { get; set; }
        public string ChoiceNum { get; set; }
        public string ChoiceName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public int? Score { get; set; }
        public bool? IsActive { get; set; }
    }
}