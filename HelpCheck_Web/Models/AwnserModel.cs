using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_Web.Models
{
    public class AwnserModel
    {
        public int QuestionID { get; set; }
        public string QuestionNum { get; set; }
        public int ChoiceID { get; set; }
        public string ChoiceNum { get; set; }
        public string AnswerKeyIn { get; set; }

    }
}
