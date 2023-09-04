using System;

namespace HelpCheck_API.Dtos.AmedQuestionMasters
{
    public class EditAmedQuestionMasterDto
    {
        internal int ID { get; set; }
        public string QuestionNum { get; set; }
        public string QuestionName { get; set; }
        //public int? ChoiceID { get; set; }
        public DateTime? QuestionPeriod { get; set; }
        internal string AccessToken { get; set; }
        public bool? IsUse { get; set; }
    }
}