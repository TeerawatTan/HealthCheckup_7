using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_Web.Models
{
    public class UserInfoModel
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
        public string Agency { get; set; }
        public string AgencyText { get; set; }
        public int? WorkPlaceID { get; set; }
        public string WorkPlaceName { get; set; }
        public int? JobTypeID { get; set; }
        public string JobTypeName { get; set; }
        public string PhoneNo { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int? RoleID { get; set; }
        public string Hn { get; set; }
        public int Count { get; set; }
        public int? TreatmentID { get; set; }
        public string TreatmentName { get; set; }
        public string Question40 { get; set; }
        public string Question2q { get; set; }
        public int Question9q { get; set; }
        public string imageUrl { get; set; }
        public string imageBase64 { get; set; }
        public int questionSt5 { get; set; }
        public int question8q { get; set; }
        public int questionGHQ28Group1 { get; set; }
        public int questionGHQ28Group2 { get; set; }
        public int questionGHQ28Group3 { get; set; }
        public int questionGHQ28Group4 { get; set; }
    }

    public class AddUserDto
    {
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
        //public int? AgencyID { get; set; }
        public int? WorkPlaceID { get; set; }
        public int? JobTypeID { get; set; }
        public string PhoneNo { get; set; }
        public string Hn { get; set; }
        public int? RoleID { get; set; }
        public int? TreatmentID { get; set; }
    }
}
