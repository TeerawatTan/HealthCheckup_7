using System;

namespace HelpCheck_API.Dtos.AmedQuestionMasters
{
    public class GetAmedQuestionMasterDto
    {
        public int ID { get; set; }
        public string QuestionNum { get; set; }
        public string QuestionName { get; set; }
        public int? ChoiceID { get; set; }
        public DateTime? QuestionPeriod { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool? IsUse { get; set; }
    }

    public class GetAmedQuestionMapChoiceMasterDto
    {
        public int ID { get; set; }
        public string QuestionNum { get; set; }
        public string QuestionName { get; set; }
        public int? ChoiceID { get; set; }
        public string ChoiceName { get; set; }
        public DateTime? QuestionPeriod { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int? Score { get; set; }
        public bool? IsUse { get; set; }
    }
}