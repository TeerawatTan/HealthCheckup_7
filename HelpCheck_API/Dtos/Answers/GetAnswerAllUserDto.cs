using HelpCheck_API.Dtos.QuestionAndChoices;
using System;

namespace HelpCheck_API.Dtos.Answers
{
    public class GetAnswerAllUserDto
    {
        public string MemberName { get; set; }
        public string MemberIdCard { get; set; }
        public string MemberHn { get; set; }
        public string QuestionName { get; set; }
        public string ChoiceName { get; set; }
        public string AsnwerKeyIn { get; set; }
        internal DateTime? CreatedDate { get; set; }
        public int? Score { get; set; }
    }
}
