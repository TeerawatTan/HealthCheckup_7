using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_Web.Models
{
    public class AnswerModel
    {
        public int MemberID { get; set; }
        public int QuestionID { get; set; }
        public string QuestionNum { get; set; }
        public string QuestionName { get; set; }
        public int ChoiceID { get; set; }
        public string ChoiceNum { get; set; }
        public string ChoiceName { get; set; }
        public string AsnwerKeyIn { get; set; }
    }
}
