namespace HelpCheck_API.Dtos.Users
{
    public class SetPasswordDto
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
    }
}
