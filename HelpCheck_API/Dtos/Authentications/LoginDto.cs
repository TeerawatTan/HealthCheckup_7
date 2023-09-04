using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Dtos.Users
{
    public class LoginRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
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

    public class DUserDetailDto
    {
        public string Name { get; set; }
        public string User { get; set; }
        public string Checkword { get; set; }
        public string Role { get; set; }
    }

    public class DUserDto
    {
        public List<DUserDetailDto> Data { get; set; }
    }
}
