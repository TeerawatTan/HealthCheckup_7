namespace HelpCheck_API.Dtos.QuestionAndChoices
{
    public class EditAnswerDto
    {
        internal string AccessToken { get; set; }
        public int QuestionID { get; set; }
        public string QuestionNum { get; set; }
        public decimal AnswerKeyIn { get; set; }
    }

    public class EditChoiceDto
    {
        public int ChoiceID { get; set; }
        public string ChoiceNum { get; set; }
        public string ChoiceName { get; set; }
    }
}