namespace HelpCheck_API.Dtos.AmedQuestionMasters
{
    public class AddAmedQuestionMasterDto
    {
        public string QuestionNum { get; set; }
        public string QuestionName { get; set; }
        public int? ChoiceID { get; set; }
        internal string AccessToken { get; set; }
        public bool? IsUse { get; set; }
    }
}