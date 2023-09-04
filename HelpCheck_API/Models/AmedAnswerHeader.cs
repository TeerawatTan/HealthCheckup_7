using System;
using System.ComponentModel.DataAnnotations;
using HelpCheck_API.Dtos.QuestionAndChoices;
using Microsoft.EntityFrameworkCore;

namespace HelpCheck_API.Models
{
    public class AmedAnswerHeader
    {
        [Key]
        public int ID { get; set; }
        public string QuestionNum { get; set; }
        public string ChoiceNum { get; set; }
        public DateTime? AnswerPeriod { get; set; }
        public int? AnswerMemberID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

    }
}