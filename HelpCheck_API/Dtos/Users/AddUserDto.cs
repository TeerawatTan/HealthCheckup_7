using Microsoft.AspNetCore.Http;
using System;

namespace HelpCheck_API.Dtos.Users
{
    public class AddUserDto
    {
        internal string AccessToken { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IDCard { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Gender { get; set; }
        public string Email { get; set; }
        public string Agency { get; set; }
        public int? WorkPlaceID { get; set; }
        public int? JobTypeID { get; set; }
        public string PhoneNo { get; set; }
        public string Hn { get; set; }
        public int? RoleID { get; set; }
        public int? TreatmentID { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
