namespace HelpCheck_API.Dtos.Psychiatrists
{
    public class EditPsychiatristCheckDto
    {
        internal string AccessToken { get; set; }
        public int MemberID { get; set; }
        public string Detail { get; set; }
    }
}
