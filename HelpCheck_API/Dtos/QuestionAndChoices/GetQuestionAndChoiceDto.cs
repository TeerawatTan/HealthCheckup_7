using System;
using System.Collections.Generic;

namespace HelpCheck_API.Dtos.QuestionAndChoices
{
    public class GetQuestionHeaderDto
    {
        public int QuestionID { get; set; }
        public string QuestionNum { get; set; }
        public string QuestionName { get; set; }
        public string ChoiceNum { get; set; }
    }

    public class GetQuestionAndChoiceDto
    {
        public int QuestionID { get; set; }
        public string QuestionNum { get; set; }
        public string QuestionName { get; set; }
        public List<GetChoiceDto> Choices { get; set; }
    }

    public class GetChoiceDto
    {
        public string QuestionNum { get; set; }
        public int ChoiceID { get; set; }
        public string ChoiceNum { get; set; }
        public string ChoiceName { get; set; }
        public int? Score { get; set; }
    }

    public class GetAnswerDto
    {
        public int MemberID { get; set; }
        public int QuestionID { get; set; }
        public string QuestionNum { get; set; }
        public string QuestionName { get; set; }
        public int ChoiceID { get; set; }
        public string ChoiceNum { get; set; }
        public string ChoiceName { get; set; }
        public string AsnwerKeyIn { get; set; }
        internal DateTime? CreatedDate { get; set; }
        public int? Score { get; set; }
    }
}