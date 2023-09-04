using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_Web.Models
{
    public class SetPasswordDto
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class LoginRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class BookingRequestDto
    {
        public string DoctorID { get; set; }
        public string AppointmentID { get; set; }
    }

    public class LoginResponseDto
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpiresIn { get; set; }
        public DateTime ExpireDate { get; set; }
        public int? RoleID { get; set; }
        //public bool IsActivated { get; set; }
    }

    public class ForgotPasswordDto
    {
        public string Username { get; set; }
    }
}
