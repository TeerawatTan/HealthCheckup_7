using System;
using System.ComponentModel.DataAnnotations;

namespace HelpCheck_API.Dtos.Users
{
    public class AddUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int? TitleID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string IDCard { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Gender { get; set; }
        public string Email { get; set; }
        public string Agency { get; set; }
        //public int? AgencyID { get; set; }
        public int? WorkPlaceID { get; set; }
        public int? JobTypeID { get; set; }
        public string PhoneNo { get; set; }
        public string Hn { get; set; }
        public int? RoleID { get; set; }
    }
}
