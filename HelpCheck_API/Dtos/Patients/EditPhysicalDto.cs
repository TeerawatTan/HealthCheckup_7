namespace HelpCheck_API.Dtos.Patients
{
    public class EditPhysicalDto
    {
        internal string AccessToken { get; set; }
        public int MemberID { get; set; }
        public int PhysicalChoiceID { get; set; }
        public int PhysicalSetID { get; set; }
        public string Value { get; set; }
    }
}