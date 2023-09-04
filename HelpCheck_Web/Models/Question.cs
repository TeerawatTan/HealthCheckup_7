using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_Web.Models
{
    public class QuestionAnsModel
    {
        public int QuestionID { get; set; }
        public string QuestionNum { get; set; }
        public string QuestionName { get; set; }
        public List<ChoiceModel> Choices { get; set; }
        public bool Isvisible { get; set; }

        QuestionAnsModel()
        {
            Choices = new List<ChoiceModel>();
        }
    }
    public class ChoiceModel
    {
        public string QuestionNum { get; set; }
        public int ChoiceID { get; set; }
        public string ChoiceNum { get; set; }
        public string ChoiceName { get; set; }

        public bool IsChecked { get; set; }
        public bool Isvisible { get; set; }
    }
}
