namespace HelpCheck_API.Dtos.QuestionAndChoices
{
    public class AddAnswerDto
    {
        public int QuestionID { get; set; }
        public string QuestionNum { get; set; }
        public int ChoiceID { get; set; }
        public string ChoiceNum { get; set; }
        public string AnswerKeyIn { get; set; }
    }
}