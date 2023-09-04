using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_Web.Models
{
    public class UserShowModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int ID { get; set; }
        public string UserID { get; set; }
        public int? TitleID { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string IDCard { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthDateStr { get; set; }
        public int Age { get; set; }
        public int? Gender { get; set; }
        public string GenderName { get; set; }
        public string Agency { get; set; }
        public int? WorkPlaceID { get; set; }
        public string WorkPlaceName { get; set; }
        public int? JobTypeID { get; set; }
        public string JobTypeName { get; set; }
        public string PhoneNo { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int? RoleID { get; set; }
    }
}
