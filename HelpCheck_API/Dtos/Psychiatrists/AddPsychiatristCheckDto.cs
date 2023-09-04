namespace HelpCheck_API.Dtos.Psychiatrists
{
    public class AddPsychiatristCheckDto
    {
        internal string AccessToken { get; set; }
        public int MemberID { get; set; }
        public int DoctorID { get; set; }
        public string Detail { get; set; }
    }
}
